using Assessment.Models.EntityModel;
using Assessment.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;

namespace Assessment.Infrastructure.DataProvider
{
    public interface ITweetDataProvider
    {
        ServiceResponse GetTweetById(long id);
        ServiceResponse AddTweet(TweetAddRequestModel model, UserTable currentUser);
        ServiceResponse ReTweetById(long id, UserTable currentUser);
        ServiceResponse DeleteTweetById(long id, UserTable currentUser);
        ServiceResponse LikeTweetById(long id, UserTable currentUser);
        ServiceResponse UnLikeTweetById(long id, UserTable currentUser);
    }
    public class TweetDataProvider : ITweetDataProvider
    {
        public ServiceResponse GetTweetById(long id)
        {
            ServiceResponse response = new ServiceResponse();

            var _context = new Entities();
            var tweet = _context.TweetPosts.SingleOrDefault(x => x.TweetId == id);
            if (tweet == null)
                response.Message = "Not found tweet";

            response.IsSuccess = true;
            response.Data = tweet;
            return response;
        }

        public ServiceResponse AddTweet(TweetAddRequestModel model, UserTable currentUser)
        {
            ServiceResponse response = new ServiceResponse();

            var _context = new Entities();
            var saveModel = new TweetPost();
            saveModel.Message = model.Message;
            saveModel.IsDelete = false;
            saveModel.CreatedBy = currentUser.UserId;
            saveModel.CreatedDate = DateTime.UtcNow;
            saveModel.ModifiedBy = currentUser.UserId;
            saveModel.ModifiedDate = DateTime.UtcNow;

            _context.TweetPosts.AddOrUpdate(saveModel);
            _context.SaveChanges();

            response.IsSuccess = true;
            response.Data = saveModel;
            response.Message = "The Tweet has been successfully added";
            return response;
        }

        public ServiceResponse ReTweetById(long id, UserTable currentUser)
        {
            ServiceResponse response = new ServiceResponse();

            var _context = new Entities();
            var saveModel = _context.TweetPosts.SingleOrDefault(x => x.TweetId == id && x.IsDelete);
            if (saveModel == null)
            {
                response.IsSuccess = false;
                response.Message = "tweet live or not found ";
                return response;
            }

            saveModel.IsDelete = false;
            saveModel.ModifiedBy = currentUser.UserId;
            saveModel.ModifiedDate = DateTime.UtcNow;

            _context.TweetPosts.AddOrUpdate(saveModel);
            _context.SaveChanges();

            response.IsSuccess = true;
            response.Data = saveModel;
            response.Message = "SuccessfullyT Re-Tweet";
            return response;
        }

        public ServiceResponse DeleteTweetById(long id, UserTable currentUser)
        {
            ServiceResponse response = new ServiceResponse();

            var _context = new Entities();
            var saveModel = _context.TweetPosts.SingleOrDefault(x => x.TweetId == id && !x.IsDelete);
            if (saveModel == null)
            {
                response.IsSuccess = false;
                response.Message = "tweet already or not found ";
                return response;
            }
            saveModel.IsDelete = true;
            saveModel.ModifiedBy = currentUser.UserId;
            saveModel.ModifiedDate = DateTime.UtcNow;

            _context.TweetPosts.AddOrUpdate(saveModel);
            _context.SaveChanges();
            response.IsSuccess = true;
            response.Message = "Successfully delete tweet";
            return response;
        }

        public ServiceResponse LikeTweetById(long id, UserTable currentUser)
        {
            ServiceResponse response = new ServiceResponse();
            var _context = new Entities();
            var tweet = _context.TweetPosts.SingleOrDefault(x => x.TweetId == id);
            if (tweet == null)
            {
                response.IsSuccess = false;
                response.Message = "Not found tweet";
                return response;
            }
            var likTweet = _context.TweetLikes.SingleOrDefault(x => x.TweetId == id && x.LikedBy == currentUser.UserId);
            if (likTweet != null)
            {
                response.IsSuccess = false;
                response.Message = "you already liked this tweet";
                return response;
            }

            var saveModel = new TweetLike();
            saveModel.TweetId = tweet.TweetId;
            saveModel.LikedBy = currentUser.UserId;
            saveModel.CreatedDate = DateTime.UtcNow;

            _context.TweetLikes.AddOrUpdate(saveModel);
            _context.SaveChanges();

            response.IsSuccess = true;
            response.Message = "Successfully Like tweet";
            return response;
        }

        public ServiceResponse UnLikeTweetById(long id, UserTable currentUser)
        {
            ServiceResponse response = new ServiceResponse();

            var _context = new Entities();
            var tweet = _context.TweetLikes.SingleOrDefault(x => x.TweetId == id && x.LikedBy == currentUser.UserId);
            if (tweet == null)
            {
                response.IsSuccess = false;
                response.Message = "Not like tweet";

                return response;
            }
            _context.TweetLikes.Remove(tweet);
            _context.SaveChanges();

            response.IsSuccess = true;
            response.Message = "Successfully UnLike tweet";
            return response;
        }
    }
}