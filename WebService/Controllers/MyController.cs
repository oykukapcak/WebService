using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebService.Controllers
{
    public class MyController : ApiController
    {
        sudoskappdbEntities ctx = new sudoskappdbEntities();

        [Route("sudosk/getitems")]
        [HttpGet]
        public List<Item> GetItems()//string name, int categoryId, string description, int count)
        {

            return ctx.Items.ToList();


        }

        [Route("sudosk/getitemsbycat")]
        [HttpGet]
        public List<Item> GetItemsByCategoryId(int categoryId)
        {

            //return ctx.Items.(itm => itm.CategoryId == categoryId);
            //List<Item> itemsbycat = new List<Item>();
            var q = (from u in ctx.Items
                     where u.CategoryId == categoryId
                     select u).ToList();
            /*foreach (var user in q)
            {
                itemsbycat.Add(user);
            }*/

            return q;
        }

        [Route("sudosk/getitem")]  //getitem by itemid
        [HttpGet]
        public Item GetItem(int itemId)
        {
            var q = (from i in ctx.Items
                     where i.Id == itemId
                     select i).FirstOrDefault();
            return q;
        }

        [Route("sudosk/getcategories")]  //get categories
        [HttpGet]
        public List<Category> GetCategories() //bunun döndüğü liste items dönmeli
        {
            return ctx.Categories.ToList();
        }

        [Route("sudosk/getrequesteditems")]  //get requesteditems
        [HttpGet]
        public List<RequestedItem> GetRequestedItems() //bunun döndüğü liste items dönmeli
        {
            return ctx.RequestedItems.ToList();
        }


        [Route("sudosk/getconfirmeditems")]  //get confirmeditems
        [HttpGet]
        public List<ConfirmedItem> GetConfirmedItems()
        {
            return ctx.ConfirmedItems.ToList();
        }


        [Route("sudosk/getbookeditems")]  //get bookeditems
        [HttpGet]
        public List<BookedItem> GetBookedItems()
        {
            return ctx.BookedItems.ToList();
        }

        [Route("sudosk/getevents")]  //get bookeditems
        [HttpGet]
        public List<Event> GetEvents()
        {
            return ctx.Events.ToList();
        }

        [Route("sudosk/getevent")]  //get bookeditems
        [HttpGet]
        public Event GetEvent(int eventId)
        {
            var q = (from i in ctx.Events
                     where i.Id == eventId
                     select i).FirstOrDefault();

            return q;
        }

        [Route("sudosk/getitemsbyuser")]
        [HttpGet]
        public List<Item> GetItemsByUserId(int userId)
        {
            var q = (from i in ctx.Items
                     where i.UserId == userId
                     select i).ToList();

            return q;
        }

        [Route("sudosk/getrequesteditem")]
        [HttpGet]
        public List<RequestedItem> GetReqItemByUserId(int userId)
        {
            var q = (from i in ctx.RequestedItems
                     where i.UserId == userId
                     select i).ToList();

            return q;
        }

        [Route("sudosk/getconfirmeditem")]
        [HttpGet]
        public List<ConfirmedItem> GetConItemByUserId(int userId)
        {
            var q = (from i in ctx.ConfirmedItems
                     where i.UserId == userId
                     select i).ToList();

            return q;
        }

        [Route("sudosk/getbookeditem")]
        [HttpGet]
        public List<BookedItem> GetBookItemByUserId(int userId)
        {
            var q = (from i in ctx.BookedItems
                     where i.UserId == userId
                     select i).ToList();

            return q;
        }

        [Route("sudosk/additem")]
        [HttpGet]
        public Item AddItem(string name, int categoryId, string description)
        {
            List<Item> list = new List<Item>();
            Item item = new Item();
            item.Name = name;
            item.CategoryId = categoryId;
            item.Description = description;
            item.Status = 0;

            ctx.Items.Add(item);
            ctx.SaveChanges();
            list.Add(item);
            return item;
        }

        [Route("sudosk/addevent")]
        [HttpGet]
        public Event AddEvent(string name)
        {
            Event myEvent = new Event();
            myEvent.Name = name;

            ctx.Events.Add(myEvent);
            ctx.SaveChanges();
            return myEvent;
        }

        [Route("sudosk/addcategory")]
        [HttpGet]
        public Category AddCategory(string name)
        {
            Category category = new Category();
            category.Name = name;

            ctx.Categories.Add(category);
            ctx.SaveChanges();
            return category;
        }

        [Route("sudosk/addmemberrequest")]
        [HttpGet]
        public MemberRequest AddMemberRequest(string name, string surname, string email)
        {
            MemberRequest memberRequest = new MemberRequest();
            memberRequest.Name = name;
            memberRequest.Surname = surname;
            memberRequest.Email = email;

            ctx.MemberRequests.Add(memberRequest);
            ctx.SaveChanges();
            return memberRequest;
        }

        [Route("sudosk/getmemberrequests")]  //get memberrequests
        [HttpGet]
        public List<MemberRequest> GetMemberRequests()
        {
            return ctx.MemberRequests.ToList();
        }


        [Route("sudosk/adduser")]
        [HttpGet]
        public User AddUser(string email)
        {
            User user = new User();

            var q = (from u in ctx.MemberRequests
                     where u.Email == email
                     select u).FirstOrDefault();

            user.Name = q.Name;
            user.Surname = q.Surname;
            user.Email = q.Email;
            user.Password = "12345";

            ctx.Users.Add(user);
            ctx.SaveChanges();

            ctx.MemberRequests.Remove(q);
            ctx.SaveChanges();
            return user;
        }

        [Route("sudosk/ondelivered")]
        [HttpGet]
        public Item OnDelivered(int itemId)
        {
            var q = (from i in ctx.BookedItems
                     where i.ItemId == itemId
                     select i).FirstOrDefault();

            ctx.BookedItems.Remove(q);

            var p = (from i in ctx.Items
                     where i.Id == itemId
                     select i).FirstOrDefault();

            p.Status = 0;
            ctx.SaveChanges();

            return p; 
        }

        [Route("sudosk/onrequest")]
        [HttpGet]
        public RequestedItem OnRequest(int itemId, string beginDate, string endDate, int eventId, int userId)
        {
            DateTime dt_1 = Convert.ToDateTime(beginDate);
            DateTime dt_2 = Convert.ToDateTime(endDate);

            var q = (from i in ctx.Items
                     where i.Id == itemId
                     select i).FirstOrDefault();

            var t = (from e in ctx.Events
                     where e.Id == eventId
                     select e).FirstOrDefault();

            var k = (from u in ctx.Users
                     where u.Id == userId
                     select u).FirstOrDefault();

            RequestedItem item = new RequestedItem();
            item.ItemId = q.Id;
            item.EventId = t.Id;
            item.UserId = k.Id;
            item.BeginDate = dt_1;
            item.EndDate = dt_2;
            item.ItemId = itemId;
            item.EventId = eventId;
            item.UserId = userId;

            ctx.RequestedItems.Add(item);
            ctx.SaveChanges();

            var p = (from i in ctx.Items
                     where i.Id == itemId
                     select i).FirstOrDefault();

            p.Status = 1;
            ctx.SaveChanges();

            return item ;
        }

        [Route("sudosk/onbook")]
        [HttpGet]
        public BookedItem OnBook(int itemId)
        {
            var q = (from i in ctx.ConfirmedItems
                     where i.ItemId == itemId
                     select i).FirstOrDefault();

            BookedItem item = new BookedItem();
            item.ItemId = itemId;
            item.BeginDate = q.BeginDate;
            item.EndDate = q.EndDate;
            item.EventId = q.EventId;
            item.UserId = q.UserId;

            ctx.ConfirmedItems.Remove(q);
            ctx.BookedItems.Add(item);

            var p = (from i in ctx.Items
                     where i.Id == itemId
                     select i).FirstOrDefault();

            p.Status = 3;
            ctx.SaveChanges();

            return item;
        }

        [Route("sudosk/onconfirm")]
        [HttpGet]
        public ConfirmedItem OnConfirm(int itemId)
        {
            var q = (from i in ctx.RequestedItems
                     where i.ItemId == itemId
                     select i).FirstOrDefault();

            ConfirmedItem item = new ConfirmedItem();
            item.ItemId = itemId;
            item.BeginDate = q.BeginDate;
            item.EndDate = q.EndDate;
            item.EventId = q.EventId;
            item.UserId = q.UserId;

            ctx.RequestedItems.Remove(q);

            ctx.SaveChanges();
            ctx.ConfirmedItems.Add(item);
            ctx.SaveChanges();

            var p = (from i in ctx.Items
                     where i.Id == itemId
                     select i).FirstOrDefault();

            p.Status = 2;
            ctx.SaveChanges();

            return item;
        }



    }
}
