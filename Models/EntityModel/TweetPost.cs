//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Assessment.Models.EntityModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class TweetPost
    {
        public long TweetId { get; set; }
        public string Message { get; set; }
        public long CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
    }
}
