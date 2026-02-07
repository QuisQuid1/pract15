using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRPO_pr15.Data;
using TRPO_pr15.Models;
using TRPO_pr15.Pages;

namespace TRPO_pr15.Services
{
    public class ProductTagService
    {
        private readonly AppDbContext _db = BaseDbService.Instance.Context;

        public ObservableCollection<Product> Products { get; set; } = new ObservableCollection<Product>();
        public ObservableCollection<Tag> Tags { get; set; } = new ObservableCollection<Tag>();

        public ProductTagService()
        {
            LoadData();
        }

        public void LoadData()
        {
            var products = _db.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Tags)
                .ToList();

            var tags = _db.Tags
                .Include(t => t.Products)
                .ToList();

            Products.Clear();
            foreach (var product in products)
            {
                Products.Add(product);
            }

            Tags.Clear();
            foreach (var tag in tags)
            {
                Tags.Add(tag);
            }
        }

        public void AddTagToProduct(int productId, int tagId)
        {
            var product = _db.Products
                .Include(p => p.Tags)
                .FirstOrDefault(p => p.Id == productId);

            var tag = _db.Tags.Find(tagId);

            if (product != null && tag != null)
            {
                if (!product.Tags.Any(t => t.Id == tagId))
                {
                    product.Tags.Add(tag);
                    _db.SaveChanges();
                    LoadData(); 
                }
            }
        }

        public void RemoveTagFromProduct(int productId, int tagId)
        {
            var product = _db.Products
                .Include(p => p.Tags)
                .FirstOrDefault(p => p.Id == productId);

            if (product != null)
            {
                var tagToRemove = product.Tags.FirstOrDefault(t => t.Id == tagId);
                if (tagToRemove != null)
                {
                    product.Tags.Remove(tagToRemove);
                    _db.SaveChanges();
                    LoadData(); 
                }
            }
        }
    }
}
