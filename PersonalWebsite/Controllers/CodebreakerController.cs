using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using PersonalWebsite.Enumerables;

namespace PersonalWebsite.Controllers
{
   public class CodebreakerController : BaseController
   {
      #region Fields

      private static List<CodebreakerCode> _codes = new List<CodebreakerCode>();
      private static string _charIndexString = "0123456789abcdef";
      private static int _codeLength = 8;

      #endregion

      #region Constructor

      public CodebreakerController(IHttpContextAccessor httpContextAccessor): base(httpContextAccessor)
      {
      }

      #endregion

      #region Methods

      [HttpGet("makeGuess")]
      public ReturnScore MakeGuess(string guess) {

         guess = guess.ToLower();
         char[] guessPieces = guess.ToCharArray();
         List<int> nums = new List<int>();
         foreach (char s in guessPieces) {
            int pegIndex = _charIndexString.IndexOf(s.ToString());
            nums.Add(pegIndex);
         }
         int[] pegs = nums.ToArray();

         if (pegs.Length != _codeLength) throw new ArgumentException("There may be only be 4 pegs submitted to CheckCode");

         foreach (int peg in pegs){
            if (peg < 0 || peg > 15){
               throw new ArgumentException("Wrong peg value, peg values may only be 0 through 9 and a through f");
            }
         }

         //get the current user's code
         CodebreakerCode code = GetCode();
         if (code == null) {
            NewCode();
            code = GetCode();
         }

         // compare the guess and actual code
         ReturnScore rScore = new ReturnScore{
            Bulls = 0,
            Cows = 0
         };

         // count bulls (correctly positioned pegs)
         for (int i = 0; i < pegs.Length; i++){
            if (pegs[i] == code.Code[i]) rScore.Bulls++;
         }

         // count cows (correct pegs in wrong positions)
         for (int i = 0; i < _charIndexString.Length; i++){
            int guessPegCount = pegs.Count(checkMe => { 
                  return checkMe==i; 
               });
            int codePegCount = code.Code.Count(checkMe => { 
                  return checkMe==i; 
               });
            int countOfCowsForOneColor = Math.Min(
               guessPegCount, 
               codePegCount
            );
            rScore.Cows += countOfCowsForOneColor;
         }
         rScore.Cows -= rScore.Bulls;

         code.Guesses.Add(guess);
         code.RScores.Add(rScore);

         // if correctly guessed, generate new code
         if (rScore.Bulls == _codeLength) NewCode();

         return rScore;

      }

      /// <summary>
      /// Generates a new code which is stored server side.
      /// </summary>
      [HttpGet("newCode")]
      public bool NewCode() {

         _codes.RemoveAll(pred => pred.CookieId == GetCookieById(CookieKeys.RngId));

         CodebreakerCode newCode = new CodebreakerCode(GetCookieById(CookieKeys.RngId), _rng, _charIndexString, _codeLength);

         _codes.Add(newCode);

         return true;

      }

      /// <summary>
      /// Generates a new code which is stored server side.
      /// </summary>
      [HttpGet("getFeedback")]
      public object GetFeedback() {

         List<ReturnScore> rScores = new List<ReturnScore>();
         List<string> guesses = new List<string>();

         CodebreakerCode code = GetCode();

         return new { 
            rScores = code.RScores.ToArray(), 
            guesses = code.Guesses.ToArray()
         };
      }

      private CodebreakerCode GetCode(){

         int index = _codes.FindIndex(0, _codes.Count, s => {
            return s.CookieId == GetCookieById(CookieKeys.RngId);
         });
         if (index == -1) return null;

         return _codes[index];
      }

      #endregion

   }

   public class ReturnScore {

      /// <summary>
      /// The number of correctly positioned pegs
      /// </summary>
      public int Bulls { get; set; }
      
      /// <summary>
      /// The number of correct pegs not correctly positioned
      /// </summary>
      public int Cows { get; set; }

   }
   
   public class CodebreakerCode {

      /// <summary>
      /// Generate a brand new Mastermind Code
      /// </summary>
      /// <param name="cookieId">The cookie id of the user</param>
		public CodebreakerCode(string cookieId, Random rng, string indexString, int len)
		{
			CookieId = cookieId;

         List<int> pegs = new List<int>();

         for (int i = 0; i < len; i++){
            pegs.Add(rng.Next(indexString.Length));
         }
         Code = pegs.ToArray();
         Guesses = new List<string>{};
         RScores = new List<ReturnScore>{};
		}

		public int[] Code { get; set; }
      public string CookieId { get; set; }
      public List<string> Guesses { get; set; }
      public List<ReturnScore> RScores { get; set; }

   }
}