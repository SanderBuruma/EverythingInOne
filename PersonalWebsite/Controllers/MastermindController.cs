using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using PersonalWebsite.Enumerables;

namespace PersonalWebsite.Controllers
{
   public class MastermindController : BaseController
   {
      private List<MastermindCode> _codes;

      public MastermindController(IHttpContextAccessor httpContextAccessor): base(httpContextAccessor)
      {
         _codes = new List<MastermindCode>();
      }

      [HttpGet("guessAtCode")]
      public ReturnScore GuessAtCode(int[] pegs) {

         if (pegs.Length != 4) throw new ArgumentException("There may be only be 4 pegs submitted to CheckCode");

         foreach (var peg in pegs){
            if (peg < 0 || peg > 7){
               throw new ArgumentException("Wrong peg value, peg values may only be 0 through 7");
            }
         }

         MastermindCode code = GetCode();
         code.Guesses++;

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
            var countOfCowsForOneColor = Math.Min(
               pegs.Count(checkMe=>checkMe==i), 
               code.Code.Count(checkMe=>checkMe==i)
            );
            rScore.Cows += countOfCowsForOneColor;
         }
         rScore.Cows -= rScore.Bulls;

         // if correctly guessed, generate new code
         if (rScore.Bulls == 4) NewCode();

         return rScore;

      }

      /// <summary>
      /// Generates a new code which is stored server side.
      /// </summary>
      [HttpGet("newCode")]
      public MastermindCode NewCode() {

         _codes.RemoveAll(pred => pred.CookieId == GetCookieById(CookieKeys.RngId));

         List<int> pegs = new List<int>();

         for (int i = 0; i < 4; i++){
            pegs.Add(_rng.Next(8));
         }

         MastermindCode newCode = new MastermindCode{
            Code = pegs.ToArray(),
            CookieId = GetCookieById(CookieKeys.RngId),
            Guesses = 0
         };

         _codes.Add(newCode);

         return newCode;

      }

      private void SubmitScore(int tries, int unixTime) {
         throw new NotImplementedException();
      }

      private MastermindCode GetCode(){

         MastermindCode code = _codes.First(s=>s.CookieId == GetCookieById(CookieKeys.RngId));
         if (code == null) {
            code = NewCode();
         }

         return code;
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
   
   public class MastermindCode {

      public int[] Code { get; set; }
      public string CookieId { get; set; }
      public int Guesses { get; set; }

   }
}