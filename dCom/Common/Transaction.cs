using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Transaction
    {
        private ushort transactionId;
        private ushort address;
        private bool finished;

        public Transaction(ushort tranId, ushort addr, bool fin)
        {
            transactionId = tranId;
            address = addr;
            finished = fin;
        }

        public Transaction() { }

        public ushort TransactionId { get => transactionId; set => transactionId = value; }
        public ushort Address { get => address; set => address = value; }
        public bool Finished { get => finished; set => finished = value; }
    }
}
