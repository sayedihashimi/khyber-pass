namespace Sedodream.SelfPub.ConfigService.Models {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;
    using AutoMapper;
    using Sedodream.SelfPub.Common;
    using Sedodream.SelfPub.ConfigService.Models.PageModels;

    public class ObjectMapper {
        private bool mapsInitalized;
        private object lockMapsInitalized = new object();
        private static ObjectMapper instance = new ObjectMapper();

        private ObjectMapper() {
            this.mapsInitalized = false;
        }

        public static ObjectMapper Instance {
            get { return ObjectMapper.instance; }
        }

        public TDest Map<TSource, TDest>(TSource source) {
            if (source == null) { throw new ArgumentNullException("source"); }

            if (!this.mapsInitalized) {
                lock(this.lockMapsInitalized) {
                    if (!this.mapsInitalized) {
                        this.InitalizeMaps();
                    }
                }
            }

            return Mapper.Map<TSource, TDest>(source);
        }

        private void InitalizeMaps() {
            this.MapSimpleTypes();
            this.MapPackage();
        }

        private void MapPackage() {
            Mapper.CreateMap<Package, PackagePageModel>()
                .ForMember(appm => appm.Tags, opt => opt.ResolveUsing(new TagCollectionFlattner()).FromMember(t => t.Tags));
        }

    
        private void MapSimpleTypes() {
            Mapper.CreateMap<string, long>()
                .ConvertUsing(str => {
                    long longValue = default(long);
                    if (!string.IsNullOrEmpty(str)) {
                        if (!long.TryParse(str, out longValue)) {
                            longValue = default(long);
                        }
                    }

                    return longValue;
                });

            Mapper.CreateMap<string, long?>()
                .ConvertUsing(str => {
                    long? convertedValue = null;
                    if (!string.IsNullOrEmpty(str)) {
                        long result;
                        if (long.TryParse(str, out result)) {
                            convertedValue = result;
                        }
                    }

                    return convertedValue;
                });

        }
    }

    public class TagCollectionFlattner : ValueResolver<List<string>, string> {
        protected override string ResolveCore(List<string> source) {
            StringBuilder sb = new StringBuilder();

            if (source != null) {
                bool firstElement = true;
                source.ToList().ForEach(obj => {
                        if (!firstElement) {
                            sb.Append(" ");
                        }
                        sb.Append(obj);

                        firstElement = false;
                    });
            }

            return sb.ToString();
        }
    }
}