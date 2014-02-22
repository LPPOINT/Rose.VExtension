using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rose.VExtension.PluginSystem.Activation
{
    public class ActivationErrorHandler : List<ActivationStepException>
    {
        public bool IsFatal
        {
            get { return this.Any(exception => exception.IsFatal); }
        }

        public int FatalCount
        {
            get { return this.Count(exception => exception.IsFatal); }
        }

        public bool HasErrors
        {
            get { return this.Any(); }
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.AppendLine("Во время активации плагина возникли следующии ошибки: ");

            foreach (var ex in this)
            {
                builder.AppendLine(String.Format("{0}: {1} {2}", ex.GetType().Name, ex.Message,
                    ex.IsFatal ? "(FATAL)" : string.Empty));
            }

            builder.AppendLine(String.Format("Всего: {0}. Фатальных: {1}", Count, FatalCount));

            return builder.ToString();

        }
    }
}
