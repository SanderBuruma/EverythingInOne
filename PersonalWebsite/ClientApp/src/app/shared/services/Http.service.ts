import { Injectable, Inject } from '@angular/core'
import { BehaviorSubject, interval, Observable } from 'rxjs';

/**Should handle all http requests*/
@Injectable()
export class HttpService {
  //#region Fields
  /** These are associated with all past requests which haven't been cancelled (regardless of whether or not those requests have been returned) */
  private _aborts: Array<AbortController> = [];
  private _runningRequests: Promise<Response>[] = [];
  private _runningRequestsCountSubject: BehaviorSubject<number> = new BehaviorSubject(0);
  private _interval: Observable<number>;
  private _failedRequests: number = 0;
  private _areWeConnected: BehaviorSubject<boolean> = new BehaviorSubject(true);
  //#endregion

  constructor(@Inject('BASE_URL') public _baseUrl: string) {}

  //#region Properties
  public get RunningRequestsCount() {
    return this._runningRequestsCountSubject.value;
  }

  public get RunningRequestsCountObservable() {
    return this._runningRequestsCountSubject.asObservable();
  }

  public get AreWeConnectedAsObservable() {
    return this._areWeConnected.asObservable();
  }
  public set AreWeConnected(value: boolean) {
    //only put in a new value if the new value is different
    if (this._areWeConnected.value != value)
      this._areWeConnected.next(value);
  }
  //#endregion

  //#region Methods
  /**
   * HttpClient does not allow request cancellation so we use fetch instead
   * @param url the url to get from
   * @param trackMe whether to track this request (ie. whether to let the header progressbar animate while this requaest is being fetched)
   */
  public async Get<T>(url: string, trackMe = true): Promise<T> {

    let aborter: AbortController = new AbortController();
    this._aborts.push(aborter);
    let init: RequestInit =  { signal: aborter.signal };

    let fetched = fetch(url, init);
    if (trackMe)
      this.AddRunningFetchRequest(fetched, url);

    let response = await fetched;
    if (!response.ok) {
      throw response.json();
    }
    return response.json();
  }

  /**HttpClient does not allow request cancellation so we use fetch instead */
  public async Post<T>(url: string, obj: object): Promise<T> {

    let aborter: AbortController = new AbortController();
    this._aborts.push(aborter);
    let init: RequestInit =  { signal: aborter.signal };

    let fetched = fetch(url, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(obj),
      signal: init.signal
    });
    this.AddRunningFetchRequest(fetched, url);
    let response = await fetched;

    if (!response.ok) {
      throw response.json();
    }
    return response.json();
  }

  /**keep track of how many requests are still active*/
  public async AddRunningFetchRequest(request: Promise<Response>, url: string) {

    this._runningRequests.push(request);
    this._runningRequestsCountSubject.next(this._runningRequests.length);

    request.finally(() => {
      let index = this._runningRequests.findIndex(rq => rq === request);
      if (index != -1) this._runningRequests.splice(index, 1);
      this._runningRequestsCountSubject.next(this._runningRequests.length);
    })
  }

  /**Cancel all http requests without distinction.*/
  public CancelRequests()
  {
    if (this._aborts.length == 0) return;

    this._aborts.forEach((abortController, _) => {
      abortController.abort();
    });
    this._aborts = [];
  }

  /**
   * Pings the server to see if we're still online. aka connection check
   * @param waitTime the amount of time between each ping in milliseconds
   * @param waitMargin the fraction of time to wait between each interval before counting a failed ping as a failure
   * @param failuresTolerated the number of failures to tolerate before popping up the disconnected screen
   */
  public StartPinging(waitTime = 4e3, waitMargin = 2/3, failuresTolerated = 3)
  {
    //start the interval
    this._interval = interval(waitTime);
    this._interval.subscribe(_=>{

      // This checks if the ping worked or not and increases the failedRequests count if it didn't
      let stillWaiting = true;
      setTimeout(()=>{

        if (stillWaiting) {

          this._failedRequests++;

          if (this._failedRequests > failuresTolerated)
            this.AreWeConnected = false;

        }

      }, waitTime*waitMargin);

      // ping the server itself.
      this.Get<Boolean>(this._baseUrl + "request/ping", false).then(_=>{

        stillWaiting = false;
        this._failedRequests = 0;
        this.AreWeConnected = true;

      });

    })
  }
  //#endregion
}
