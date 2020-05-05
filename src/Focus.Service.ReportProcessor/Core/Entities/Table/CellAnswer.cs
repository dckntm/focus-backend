using System.Collections.Generic;
using Focus.Core.Common.Abstract;
using Focus.Service.ReportProcessor.Enums;

namespace Focus.Service.ReportProcessor.Entities.Table
{
    public class CellAnswer : ValueObject
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public string Answer { get; set; }
        public InputType AnswerType { get; set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Row;
            yield return Column;
            yield return Answer;
            yield return AnswerType;
        }
    }
}