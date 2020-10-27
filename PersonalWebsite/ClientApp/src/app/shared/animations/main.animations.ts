import { animate, state, style, transition, trigger } from '@angular/animations';
import { AnimationTimers } from 'src/app/shared/enums/animation-timers.enum';

export let fade = trigger(
  'fade',
  [
    state('void', style ({opacity: 0})),
    state('default', style ({opacity: '*'})),
    transition('void => *', [
      animate(
        AnimationTimers.Fade
      )
    ])
  ]
);
