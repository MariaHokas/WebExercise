//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebSQLAzure.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Henkilot
    {
        public int Henkiloid { get; set; }
        public string Nimi { get; set; }
        public string Sukunimi { get; set; }
        public string Katuosoite { get; set; }
        public Nullable<int> Postitoiminumero { get; set; }
        public string Postitoimipaikka { get; set; }
        public string Esimies { get; set; }
    }
}
