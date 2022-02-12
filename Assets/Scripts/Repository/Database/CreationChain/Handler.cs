namespace FCA.Data.Chain
{
    public abstract class Handler
    {
        protected Handler successor;

        public abstract void handle(Request req);

        public void setSuccesor(Handler succHandler)
        {
            this.successor = succHandler;
        }
    }
}
