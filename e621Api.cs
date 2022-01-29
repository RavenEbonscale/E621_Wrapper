using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace E621_Wrapper
{
    public class Api
    {
        private string ApiKey { get; }

        private string username { get; }

        private string useragent { get; }

        public Api(string ApiKey, string username, string useragent)
        {
            this.ApiKey = ApiKey;
            this.username = username;
            this.useragent = useragent;
        }

        internal HttpClient E621Client
        {
            get
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("api-key", ApiKey);
                client.DefaultRequestHeaders.Add("api-key", username);
                client.DefaultRequestHeaders.Add("user-agent", useragent);
                return client;
            }
        }

        ///Get post From E621 based on a list of tags
        public List<E621json> Get_Posts(string tags, int pages)
        {
            List<E621json> e621posts = new List<E621json>();
            Parallel.For(0, pages, async page =>
           {
               using (MemoryStream e621 = await $"https://e621.net/posts.json?page={page}&tags={tags}".Deserializetion(E621Client))
               {
                   E621json posts = await JsonSerializer.DeserializeAsync<E621json>(e621);
                   e621posts.Add(posts);
               }
           });
            return e621posts;
        }
        
        ///Get a list of Pools based on a name
        public async Task<List<E621pools>> Get_Pool(string pool)
        {
            List<E621pools> pools = new();

            using (MemoryStream e621 = await $"https://e621.net/pools.json?search[name_matches]={pool}".Deserializetion(E621Client))
            {
                List<E621pools> _pool = await JsonSerializer.DeserializeAsync<List<E621pools>>(e621);
                pools.AddRange(_pool);
            }

            return pools;
        }
        ///Get a list of Pools based on a Id
        public async Task<E621poolid> Get_PoolId(int poolId)
        {

            using MemoryStream e621 = await $"https://e621.net/pools/{poolId}.json?".Deserializetion(E621Client);
            E621poolid _pool = await JsonSerializer.DeserializeAsync<E621poolid>(e621);

            return _pool;
        }

        public async Task<Singlepost> Get_Id(int id)
        {
            MemoryStream e621 = await $"https://e621.net/posts/{id}.json".Deserializetion(E621Client);
            
           var post = await JsonSerializer.DeserializeAsync<Singlepost>(e621);

            

            return post;
        }
        public async Task <E621json> Get_Favs()
        {
            
            using MemoryStream e621 = await $"https://e621.net/favorites.json".Deserializetion(E621Client);
            E621json posts = await JsonSerializer.DeserializeAsync<E621json>(e621);

            return posts;
        }
 
        ///For when you want only  images that are SFW
        
        ///
        public List<E621json> Get_Posts_Sfw(string tags, int pages)
        {
            List<E621json> e621posts = new List<E621json>();
            Parallel.For(0, pages, async page =>
            {
                using (MemoryStream e621 = await $"https://e926.net/posts.json?page={page}&tags={tags}".Deserializetion(E621Client))
                {
                    E621json posts = await JsonSerializer.DeserializeAsync<E621json>(e621);
                    e621posts.Add(posts);
                }
            });
            return e621posts;
        }
     
        public async Task<List<E621pools>> Get_Pool_Sfw(string pool)
        {
            List<E621pools> pools = new();

            using (MemoryStream e621 = await $"https://e926.net/pools.json?search[name_matches]={pool}".Deserializetion(E621Client))
            {
                var _pool = await JsonSerializer.DeserializeAsync<List<E621pools>>(e621);
                pools.AddRange(_pool);
            }

            return pools;
        }
      
        public async Task<Singlepost> Get_Id_Sfw(int id)
        {
            MemoryStream e926 = await $"https://e926.net/posts/{id}.json".Deserializetion(E621Client);
            //Console.WriteLine(Encoding.ASCII.GetString(e926.ToArray()));
            var post = await JsonSerializer.DeserializeAsync<Singlepost>(e926);



            return post;
        }
      
        public async Task<E621json> Get_Favs_Sfw()
        {

            using MemoryStream e926 = await $"https://e926.net/favorites.json".Deserializetion(E621Client);
            E621json posts = await JsonSerializer.DeserializeAsync<E621json>(e926);

            return posts;
        }
    }
}