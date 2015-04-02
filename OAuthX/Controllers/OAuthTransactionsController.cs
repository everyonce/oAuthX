using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using OAuthX.Models;
using System.Web.Http.Results;
using System.Text;

namespace OAuthX.Controllers
{
    public class OAuthTransactionsController : ApiController
    {
        private dbContext db = new dbContext();

        // GET: api/OAuthTransactions
        public IQueryable<OAuthTransaction> GetOAuthTransactions()
        {
            return db.OAuthTransactions;
        }

        // GET: api/OAuthTransactions/5
        [ResponseType(typeof(OAuthTransaction))]
        public async Task<IHttpActionResult> GetOAuthTransaction(Guid id)
        {
            OAuthTransaction oAuthTransaction = await db.OAuthTransactions.FindAsync(id);
            if (oAuthTransaction == null)
            {
                return NotFound();
            }

            return Ok(oAuthTransaction);
        }

        [ResponseType(typeof(OAuthTransaction))]
        public async Task<IHttpActionResult> GetOAuthTransaction(Guid id, String authUrl)
        {
            return await GetOAuthTransaction(id, authUrl, "other");
        }

        [ResponseType(typeof(OAuthTransaction))]
        public async Task<IHttpActionResult> GetOAuthTransaction(Guid id, String authUrl, String authType)
        {
            String newAuthUrl = Encoding.UTF8.GetString(Convert.FromBase64String(authUrl));
            OAuthTransaction oAuthTransaction = new OAuthTransaction(id, authUrl, authType);
            db.OAuthTransactions.Add(oAuthTransaction);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (OAuthTransactionExists(oAuthTransaction.id))
                {
                    //do nothing, we can still pass the url for the existing one.
                    //TODO: give helpful error, etxc
                }
                else
                {
                    throw;
                }
            }
            if (oAuthTransaction.authType == OAuthTransaction.authTypes.loginWithAmazon)
                newAuthUrl = processAmazonUrl.getLoginString(newAuthUrl, oAuthTransaction);

            return Redirect(new Uri(newAuthUrl));
        }

        //// PUT: api/OAuthTransactions/5
        //[ResponseType(typeof(void))]
        //public async Task<IHttpActionResult> PutOAuthTransaction(Guid id, OAuthTransaction oAuthTransaction)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != oAuthTransaction.id)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(oAuthTransaction).State = EntityState.Modified;

        //    try
        //    {
        //        await db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!OAuthTransactionExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        // POST: api/OAuthTransactions
        [ResponseType(typeof(HttpResponseMessage))]
        public async Task<IHttpActionResult> PostOAuthTransaction(OAuthTransaction oAuthTransaction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.OAuthTransactions.Add(oAuthTransaction);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (OAuthTransactionExists(oAuthTransaction.id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Redirect(new Uri("http://www.google.com"));
            //return CreatedAtRoute("DefaultApi", new { id = oAuthTransaction.id }, oAuthTransaction);
        }

        // DELETE: api/OAuthTransactions/5
        [ResponseType(typeof(OAuthTransaction))]
        public async Task<IHttpActionResult> DeleteOAuthTransaction(Guid id)
        {
            OAuthTransaction oAuthTransaction = await db.OAuthTransactions.FindAsync(id);
            if (oAuthTransaction == null)
            {
                return NotFound();
            }

            db.OAuthTransactions.Remove(oAuthTransaction);
            await db.SaveChangesAsync();

            return Ok(oAuthTransaction);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OAuthTransactionExists(Guid? id)
        {
            return db.OAuthTransactions.Count(e => e.id == id) > 0;
        }
    }
}