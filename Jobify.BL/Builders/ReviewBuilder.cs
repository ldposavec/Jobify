using Jobify.BL.DALModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobify.BL.Builders
{
    public class ReviewBuilder
    {
        private readonly Review _review;

        private ReviewBuilder()
        {
            _review = new Review();
        }

        public static ReviewBuilder Create()
        {
            return new ReviewBuilder();
        }

        public ReviewBuilder SetId(int id)
        {
            _review.Id = id;
            return this;
        }

        public ReviewBuilder SetFirmId(int firmId)
        {
            _review.FirmId = firmId;
            return this;
        }

        public ReviewBuilder SetFirm(Firm firm)
        {
            _review.Firm = firm;
            return this;
        }

        public ReviewBuilder SetUserId(int userId)
        {
            _review.UserId = userId;
            return this;
        }

        public ReviewBuilder SetUser(User user)
        {
            _review.User = user;
            return this;
        }

        public ReviewBuilder SetGrade(int grade)
        {
            _review.Grade = grade;
            return this;
        }

        public ReviewBuilder SetComment(string comment)
        {
            _review.Comment = comment;
            return this;
        }

        public Review Build()
        {
            return _review;
        }
    }
}
