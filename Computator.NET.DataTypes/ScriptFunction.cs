using System;
using System.Reflection;
using System.Windows.Forms;
using Computator.NET.Config;
using Computator.NET.Logging;

namespace Computator.NET.DataTypes
{
    public class ScriptFunction : BaseFunction
    {
        public ScriptFunction(Delegate function, string tslCode, string csCode)
            : base(function, tslCode, csCode)
        {
            FunctionType = FunctionType.Scripting;

            logger = new SimpleLogger {ClassName = GetType().FullName};
        }

        public void Evaluate(RichTextBox consoleOutput)
        {
            try
            {
                ((Action<RichTextBox>) _function)(consoleOutput);
            }
            catch (Exception exception)
            {
                logger.MethodName = MethodBase.GetCurrentMethod().Name;
                logger.Parameters["TSLCode"] = tslCode;
                logger.Parameters["CSCode"] = csCode;
                logger.Parameters["FunctionType"] = FunctionType;
                logger.Log(exception.Message, ErrorType.Calculation, exception);

                var message = "Calculation Error, details:" + Environment.NewLine + exception.Message;

                throw new CalculationException(message, exception);
            }
        }
    }
}