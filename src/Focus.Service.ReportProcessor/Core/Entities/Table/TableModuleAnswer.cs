using System.Collections.Generic;
using Focus.Core.Common.Abstract;

namespace Focus.Service.ReportProcessor.Entities.Table
{
    public class TableModuleAnswer : ValueObject
    {
        public string Title { get; set; }
        public int Order { get; set; }
        public IList<CellAnswer> CellAnswers { get; set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Order;

            foreach (var c in CellAnswers)
                yield return c;
        }
    }
}