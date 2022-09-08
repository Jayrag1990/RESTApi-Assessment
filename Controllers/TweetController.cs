using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Assessment.Infrastructure.Attributes;
using Assessment.Infrastructure.DataProvider;
using Assessment.Models.ViewModel;
using Assessment.Infrastructure.Helpers;

namespace Assessment.Controllers
{
    [CustomAuthorizeAttribute]
    [RoutePrefix("api/Tweet")]
    public class TweetController : BaseController
    {
        [HttpPost]
        [Route("Add")]
        public IHttpActionResult Add(TweetAddRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ITweetDataProvider _tweetDataProvider = new TweetDataProvider();
            var result = _tweetDataProvider.AddTweet(model, SessionHelper.UserDetail.CurrentUser);

            return Ok(result);
        }

        [HttpGet]
        [Route("get/{id}")]
        public IHttpActionResult Get(long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ITweetDataProvider _tweetDataProvider = new TweetDataProvider();
            var result = _tweetDataProvider.GetTweetById(id);

            return Ok(result);
        }

        [HttpPut]
        [Route("retweet/{id}")]
        public IHttpActionResult ReTweet(long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ITweetDataProvider _tweetDataProvider = new TweetDataProvider();
            var result = _tweetDataProvider.ReTweetById(id, SessionHelper.UserDetail.CurrentUser);

            return Ok(result);
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public IHttpActionResult Delete(long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ITweetDataProvider _tweetDataProvider = new TweetDataProvider();
            var result = _tweetDataProvider.DeleteTweetById(id, SessionHelper.UserDetail.CurrentUser);

            return Ok(result);
        }

        [HttpPost]
        [Route("like/{id}")]
        public IHttpActionResult Like(long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ITweetDataProvider _tweetDataProvider = new TweetDataProvider();
            var result = _tweetDataProvider.LikeTweetById(id, SessionHelper.UserDetail.CurrentUser);

            return Ok(result);
        }

        [HttpDelete]
        [Route("unlike/{id}")]
        public IHttpActionResult UnLike(long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ITweetDataProvider _tweetDataProvider = new TweetDataProvider();
            var result = _tweetDataProvider.UnLikeTweetById(id, SessionHelper.UserDetail.CurrentUser);

            return Ok(result);
        }
    }
}