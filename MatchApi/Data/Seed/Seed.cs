using MatchApi.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchApi.Data
{
    public class Seed
    {
        //private readonly AppDbContext _context;

        //public Seed(AppDbContext context)
        //{
        //    _context = context;
        //}

        public void SeedMembers()
        {
            using(var db = new AppDbContext())
            {
                var memberJson1 = System.IO.File.ReadAllText("data/Seed/member1.json");
                var members1 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Member>>(memberJson1);
                foreach (var member in members1)
                {
                    byte[] passwordHash, passwordSalt;
                    PasswordHash.CreatePasswordHash("password", out passwordHash, out passwordSalt);
                    member.PasswordHash = passwordHash;
                    member.PasswordSalt = passwordSalt;
                    member.IsCloseData = false;
                    member.IsClosePhoto = false;
                    //member.IsBlackUser = 0;
                    db.Add<Member>(member);
                    //_context.Add<Member>(member);
                }
                db.SaveChanges();

                var memberJson2 = System.IO.File.ReadAllText("data/Seed/member2.json");
                var members2 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Member>>(memberJson2);
                foreach (var member in members2)
                {
                    byte[] passwordHash, passwordSalt;
                    PasswordHash.CreatePasswordHash("password", out passwordHash, out passwordSalt);
                    member.PasswordHash = passwordHash;
                    member.PasswordSalt = passwordSalt;
                    member.IsCloseData = false;
                    member.IsClosePhoto = false;
                    //member.IsBlackUser = 0;
                    db.Add<Member>(member);
                    //_context.Add<Member>(member);
                }
                db.SaveChanges();
            }
        }


        public void SeedMemberPhotos()
        {
            using (var db = new AppDbContext())
            {
                var photoJson = System.IO.File.ReadAllText("Data/seed/photo1.json");
                var photos = JsonConvert.DeserializeObject<List<MemberPhoto>>(photoJson);
                foreach (var item in photos)
                {
                    db.MemberPhoto.Add(item);
                }
                db.SaveChanges();
            }
        }


        public void SeedMemberDetail()
        {
            using (var db = new AppDbContext())
            {
                var Jsondata = System.IO.File.ReadAllText("data/Seed/memberDetail.json");
                var model = JsonConvert.DeserializeObject<List<MemberDetail>>(Jsondata);
                foreach (var item in model)
                {
                    db.MemberDetail.Add(item);
                }
                db.SaveChanges();
            }
        }

        public void SeedMemberCondition()
        {
            using (var db = new AppDbContext())
            {

                var Jsondata = System.IO.File.ReadAllText("data/Seed/memberCondition.json");
                var model = JsonConvert.DeserializeObject<List<MemberCondition>>(Jsondata);
                foreach (var item in model)
                {
                    db.MemberCondition.Add(item);
                }
                db.SaveChanges();
            }
        }


        public void SeedLiker()
        {
            using(var db = new AppDbContext())
            {
                var today = System.DateTime.Now;
                var Jsondata = System.IO.File.ReadAllText("data/seed/liker.json");
                var model = JsonConvert.DeserializeObject<List<Liker>>(Jsondata);
                foreach (var item in model)
                {
                    var liker = db.Liker.Where(x => x.LikerId == item.LikerId && x.SendId == item.SendId);
                    if (liker == null)
                    {
                        item.IsDelete = false;
                        db.Liker.Add(item);
                        db.SaveChanges();
                    }
                }

            }
        }

    }
}
