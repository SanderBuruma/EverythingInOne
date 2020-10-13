using System;
using System.Text;

namespace Cubit32.Logging
{
   public class LoggingFormatter
   {
      private readonly StringBuilder _bodyBuilder;
      private readonly StringBuilder _subjectBuilder;
      private readonly Exception _ex;

      public string Subject
      {
         get { return _subjectBuilder.ToString(); }
      }

      public string Body
      {
         get { return _bodyBuilder.ToString(); }
      }

      public LoggingFormatter(Exception ex)
      {
         _bodyBuilder = new StringBuilder();
         _subjectBuilder = new StringBuilder();
         _ex = ex;

         MakeSubject();
         MakeBody();
      }

      /// <summary>
      /// Add a standard exception email subject
      /// </summary>
      private void MakeSubject()
      {
         _subjectBuilder.Append(_ex.GetType());
         _subjectBuilder.Append(" - ");
         _subjectBuilder.Append(_ex.Message);
      }

      /// <summary>
      /// Can do lots of pushups once its done body building. Also makes a great exception email body.
      /// </summary>
      private void MakeBody()
      {
         //format the primary exception
         _bodyBuilder.Append($"<h2>");
         _bodyBuilder.Append(_ex.GetType());
         _bodyBuilder.Append(" - ");
         _bodyBuilder.Append(_ex.Message);
         _bodyBuilder.Append($"</h2>");
         if (_ex.StackTrace != null) FormatStackTrace(_ex.StackTrace);

         //and every inner exception
         Exception currentInnerException = _ex.InnerException;
         while (currentInnerException != null)
         {
            _bodyBuilder.Append($"<h2>");
            _bodyBuilder.Append(currentInnerException.GetType());
            _bodyBuilder.Append(" - ");
            _bodyBuilder.Append(currentInnerException.Message);
            _bodyBuilder.Append($"</h2>");
            if (currentInnerException.StackTrace != null) FormatStackTrace(currentInnerException.StackTrace);

            currentInnerException = currentInnerException.InnerException;
         }
      }

      private void FormatStackTrace(string stackTrace)
      {
         foreach (String stLine in stackTrace.Split("\n"))
         {
            _bodyBuilder.Append(stLine);
            _bodyBuilder.Append("<br>");
         }
      }
   }
}
