//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace BEEWiN.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Coin_Order
    {
        public string Order_Guid { get; set; }
        public string Email { get; set; }
        public string Symbol { get; set; }
        public System.DateTime Buy_Date { get; set; }
        public decimal Buy_Price { get; set; }
        public int Buy_Amount { get; set; }
        public decimal Total { get; set; }
        public string Order_Status { get; set; }
    
        public virtual Coin Coin { get; set; }
        public virtual Member Member { get; set; }
    }
}