using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotApp.Telegram.Api.Results
{
    internal class Result<TValue> 
    {
        private TValue? _value;
        private bool _isSuccess;
        private Exception? _exception;

        public TValue? Value => _value;
        public bool IsSuccess => _isSuccess;
        public bool IsFailure => !_isSuccess;
        public Exception? Exception => _exception;

        private Result() 
        {
            _value = default;
            _isSuccess = false;
            _exception = null;
        }
        private Result(TValue value)
        {
            _value = value;
            _isSuccess = true;
            _exception = null;
        }
        private Result(Exception exception)
        {
            _value = default;
            _isSuccess = false;
            _exception = exception;
        }
        
        internal static Result<TValue> Success(TValue value) => new(value);
        internal static Result<TValue> Failure(Exception exception) => new(exception);
    }
}
