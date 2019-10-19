using System;

namespace EPaperHat
{
    public abstract class BaseEPaper : IEPaper
    {
        public virtual void Initialize()
        {
            throw new NotImplementedException();
        }

        public virtual void Clear()
        {
            throw new NotImplementedException();
        }

        public virtual void Sleep()
        {
            throw new NotImplementedException();
        }
    }
}