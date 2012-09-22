using System.Collections.Generic;

namespace CaseloadManager.Models.Model
{
    public class Base
    {
        private bool _validationCalled;

        private List<string> _validationErrors;

        public Base()
        {
            _validationCalled = false;
        }

        public List<string> ValidationErrors
        {
            get
            {
                if (_validationErrors == null)
                    _validationErrors = new List<string>();

                return _validationErrors;
            }

            set
            {
                _validationErrors = value;
            }
        }

        public virtual void DoValidation()
        {
            _validationCalled = true;
        }

        public bool IsValid()
        {
            if (!_validationCalled)
            {
                DoValidation();
            }
            if (ValidationErrors.Count > 0)
            {
                return false;
            }
            return true;
        }

    }
}