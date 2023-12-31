﻿

namespace Domain.Entities
{
    public class Book : BaseEntity
    {
        public string BookTitle { get; set; }
        public int Quantity { get; set; }        
        public decimal Price { get; set; }

        public virtual Author Author { get; set; }
        public int AuthorId { get; set; }

        public virtual Publisher Publisher { get; set; }
        public int PublisherId { get; set; }
    }
}
