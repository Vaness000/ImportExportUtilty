//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UtilityEngineORM
{
    using System;
    using System.Collections.Generic;
    
    public partial class Answer
    {
        public int Answer_code { get; set; }
        public int Question_code { get; set; }
        public string Answer_text { get; set; }
        public bool Answer_flag { get; set; }
    
        public virtual Question Question { get; set; }
    }
}