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
      private static List<CodebreakerCode> _codes = new List<CodebreakerCode>();

      public CodebreakerController(IHttpContextAccessor httpContextAccessor): base(httpContextAccessor)
      {
      }

      [HttpGet("makeGuess")]
      public ReturnScore MakeGuess(string guess) {

         var guessPieces = guess.ToCharArray();
         var nums = new List<int>();
         foreach (char s in guessPieces) {
            nums.Add(s - 49);
         }
         int[] pegs = nums.ToArray();

         if (pegs.Length != 4) throw new ArgumentException("There may be only be 4 pegs submitted to CheckCode");

         foreach (var peg in pegs){
            if (peg < 0 || peg > 7){
               throw new ArgumentException("Wrong peg value, peg values may only be 0 through 7");
            }
         }

         CodebreakerCode code = GetCode();
         if (code == null) {
            NewCode();
            code = GetCode();
         }

         // compare the guess and actual code
         var rScore = new ReturnScore{
            Bulls = 0,
            Cows = 0
         };

         // count bulls
         for (int i = 0; i < pegs.Length; i++){
            if (pegs[i] == code.Code[i]) rScore.Bulls++;
         }

         // count cows
         for (int i = 0; i < 8; i++){
            var count1 = pegs.Count(checkMe => { 
                  return checkMe==i; 
               });
            var count2 = code.Code.Count(checkMe => { 
                  return checkMe==i; 
               });
            var countOfCowsForOneColor = Math.Min(
               count1, 
               count2
            );
            rScore.Cows += countOfCowsForOneColor;
         }
         rScore.Cows -= rScore.Bulls;

         code.Guesses.Add(guess);
         code.RScores.Add(rScore);

         // if correctly guessed, generate new code
         if (rScore.Bulls == 4) NewCode();

         return rScore;

      }

      /// <summary>
      /// Generates a new code which is stored server side.
      /// </summary>
      [HttpGet("newCode")]
      public bool NewCode() {

         _codes.RemoveAll(pred => pred.CookieId == GetCookieById(CookieKeys.RngId));

         CodebreakerCode newCode = new CodebreakerCode(GetCookieById(CookieKeys.RngId), _rng);

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

         var index = _codes.FindIndex(0, _codes.Count, s => {
            return s.CookieId == GetCookieById(CookieKeys.RngId);
         });
         if (index == -1) return null;

         return _codes[index];
      }

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
		public CodebreakerCode(string cookieId, Random rng)
		{
			CookieId = cookieId;

         List<int> pegs = new List<int>();

         for (int i = 0; i < 4; i++){
            pegs.Add(rng.Next(8));
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