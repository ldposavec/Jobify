using Jobify.BL.DALModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobify.BL.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly JobifyContext _context;

        public ReviewRepository(JobifyContext context)
        {
            _context = context;
        }
        public Review Delete(int id)
        {
            var review = GetById(id);
            if (review == null)
            {
                throw new Exception($"Review with id {id} not found.");
            }

            _context.Reviews.Remove(review);
            Save();

            return review;
        }

        public IEnumerable<Review> GetAll()
        {
            return _context.Reviews.ToList();
        }

        public Review? GetById(int id)
        {
            return _context.Reviews.FirstOrDefault(c => c.Id == id);
        }

        public Review? GetExistingReview(int firmId, int userId)
        {
            return _context.Reviews.FirstOrDefault(b => b.FirmId == firmId && b.UserId == userId);
        }

        public IEnumerable<Review> GetReviewsByFirmId(int firmId)
        {
            return _context.Reviews.Where(c => c.FirmId == firmId).ToList();
        }

        public void Insert(Review entity)
        {
            _context.Reviews.Add(entity);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Review entity)
        {
            _context.Reviews.Update(entity);
        }
    }
}
