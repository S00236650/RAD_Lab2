﻿namespace RAD_Lab2_Part5
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string? Name { get; set; }
        public List<Ad> ads { get; set; } = new();
    }
}
