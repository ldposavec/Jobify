using Jobify.BL.DALModels;
using Jobify.BL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobify.BL.Builders
{
    public class JobAppBuilder : JobApp, IPrototype<JobAppBuilder>
    {
        private JobAppBuilder() { }

        public JobAppBuilder Clone()
        {
            return new JobAppBuilder
            {
                Id = Id,
                JobAdId = JobAdId,
                StudentId = StudentId,
                CreatedAt = CreatedAt,
                CvFilepath = CvFilepath,
                StatusId = StatusId
            };
            //var jobApp = new JobAppBuilder.Builder()
            //    .WithJobAdId(JobAdId)
            //    .WithStudentId(StudentId)
            //    .WithCreatedAt(CreatedAt)
            //    .WithCvFilepath(CvFilepath)
            //    .WithStatusId(StatusId)
            //    .Build();

            //return jobApp;
        }

        public override string? ToString()
        {
            return $"{CvFilepath} - {CreatedAt}";
        }

        public class Builder
        {
            private JobAppBuilder _jobAppBuilder = new();

            //public Builder WithId(int id)
            //{
            //    _jobAppBuilder.Id = id;
            //    return this;
            //}
            public Builder WithJobAdId(int jobAdId)
            {
                _jobAppBuilder.JobAdId = jobAdId;
                return this;
            }
            public Builder WithStudentId(int studentId)
            {
                _jobAppBuilder.StudentId = studentId;
                return this;
            }
            public Builder WithCreatedAt(DateTime? createdAt)
            {
                if(createdAt is not null) _jobAppBuilder.CreatedAt = createdAt;
                else _jobAppBuilder.CreatedAt = DateTime.Now;
                return this;
            }
            public Builder WithCvFilepath(string cvFilepath)
            {
                _jobAppBuilder.CvFilepath = cvFilepath;
                return this;
            }
            public Builder WithStatusId(int statusId)
            {
                _jobAppBuilder.StatusId = statusId;
                return this;
            }
            public JobAppBuilder Build()
            {
                return _jobAppBuilder;
            }
        }
    }
}
