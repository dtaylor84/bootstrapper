using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Bootstrap.Extensions;
using Bootstrap.Extensions.Containers;

namespace Bootstrap.AutoMapper
{
    using System;
    using System.Reflection;
    using global::AutoMapper.Mappers;
    using global::AutoMapper.QueryableExtensions;

    public class AutoMapperExtension : IBootstrapperExtension
    {
        private readonly IRegistrationHelper registrationHelper;

        private static readonly ConfigurationProviderWrapper WrappedConfigurationProvider
            = new ConfigurationProviderWrapper();

        private static readonly ProfileExpressionWrapper WrappedProfileExpression
            = new ProfileExpressionWrapper();

        private static readonly MapperWrapper WrappedMapper = new MapperWrapper();

        private static readonly MappingEngineWrapper WrappedMappingEngine = new MappingEngineWrapper();

        public static IConfigurationProvider ConfigurationProvider => WrappedConfigurationProvider;

        public static IProfileExpression ProfileExpression => WrappedProfileExpression;

        public static IMapper Mapper => WrappedMapper;

        public static IMappingEngine Engine => WrappedMappingEngine;

        static AutoMapperExtension()
        {
            SetConfiguration(new MapperConfiguration(c => { }));
        }

        public AutoMapperExtension(IRegistrationHelper registrationHelper)
        {
            Bootstrapper.Excluding.Assembly("AutoMapper");
            this.registrationHelper = registrationHelper;
        }

        public void Run()
        {
            List<IMapCreator> mapCreators;
            List<Profile> profiles;

            if (Bootstrapper.ContainerExtension != null && Bootstrapper.Container != null)
            {
                mapCreators = Bootstrapper.ContainerExtension.ResolveAll<IMapCreator>().ToList();
                profiles = Bootstrapper.ContainerExtension.ResolveAll<Profile>().ToList();
            }
            else
            {
                mapCreators = this.registrationHelper.GetInstancesOfTypesImplementing<IMapCreator>();
                profiles = this.registrationHelper.GetInstancesOfTypesImplementing<Profile>();
            }

            SetConfiguration(new MapperConfiguration(c =>
                {
                    profiles.ForEach(c.AddProfile);
                    mapCreators.ForEach(m => m.CreateMap(c));
                }));
        }

        public void Reset()
        {
            SetConfiguration(new MapperConfiguration(c => { }));
        }

        private static void SetConfiguration(MapperConfiguration mapperConfiguration)
        {
            WrappedConfigurationProvider.ConfigurationProvider = mapperConfiguration;
            WrappedProfileExpression.ProfileExpression = mapperConfiguration;
            WrappedMapper.Mapper = mapperConfiguration.CreateMapper();
            WrappedMappingEngine.MappingEngine = new MappingEngine(ConfigurationProvider, Mapper);
        }

        private class ConfigurationProviderWrapper : IConfigurationProvider
        {
            internal IConfigurationProvider ConfigurationProvider { private get; set; }

            TypeMap[] IConfigurationProvider.GetAllTypeMaps() => this.ConfigurationProvider.GetAllTypeMaps();

            TypeMap IConfigurationProvider.FindTypeMapFor(Type sourceType, Type destinationType) => this.ConfigurationProvider.FindTypeMapFor(sourceType, destinationType);

            TypeMap IConfigurationProvider.FindTypeMapFor(TypePair typePair) => this.ConfigurationProvider.FindTypeMapFor(typePair);

            TypeMap IConfigurationProvider.FindTypeMapFor<TSource, TDestination>() => this.ConfigurationProvider.FindTypeMapFor<TSource, TDestination>();

            TypeMap IConfigurationProvider.ResolveTypeMap(object source, object destination, Type sourceType, Type destinationType) => this.ConfigurationProvider.ResolveTypeMap(source, destination, sourceType, destinationType);

            TypeMap IConfigurationProvider.ResolveTypeMap(Type sourceType, Type destinationType) => this.ConfigurationProvider.ResolveTypeMap(sourceType, destinationType);

            TypeMap IConfigurationProvider.ResolveTypeMap(TypePair typePair) => this.ConfigurationProvider.ResolveTypeMap(typePair);

            TypeMap IConfigurationProvider.ResolveTypeMap(ResolutionResult resolutionResult, Type destinationType) => this.ConfigurationProvider.ResolveTypeMap(resolutionResult, destinationType);

            IProfileConfiguration IConfigurationProvider.GetProfileConfiguration(string profileName) => this.ConfigurationProvider.GetProfileConfiguration(profileName);

            void IConfigurationProvider.AssertConfigurationIsValid() => this.ConfigurationProvider.AssertConfigurationIsValid();

            void IConfigurationProvider.AssertConfigurationIsValid(TypeMap typeMap) => this.ConfigurationProvider.AssertConfigurationIsValid(typeMap);

            void IConfigurationProvider.AssertConfigurationIsValid(string profileName) => this.ConfigurationProvider.AssertConfigurationIsValid(profileName);

            void IConfigurationProvider.AssertConfigurationIsValid<TProfile>() => this.ConfigurationProvider.AssertConfigurationIsValid<TProfile>();

            IEnumerable<IObjectMapper> IConfigurationProvider.GetMappers() => this.ConfigurationProvider.GetMappers();

            IEnumerable<ITypeMapObjectMapper> IConfigurationProvider.GetTypeMapMappers() => this.ConfigurationProvider.GetTypeMapMappers();

            Func<Type, object> IConfigurationProvider.ServiceCtor => this.ConfigurationProvider.ServiceCtor;

            bool IConfigurationProvider.AllowNullDestinationValues => this.ConfigurationProvider.AllowNullDestinationValues;

            bool IConfigurationProvider.AllowNullCollections => this.ConfigurationProvider.AllowNullCollections;

            IExpressionBuilder IConfigurationProvider.ExpressionBuilder => this.ConfigurationProvider.ExpressionBuilder;
        }

        private class ProfileExpressionWrapper : IProfileExpression
        {
            internal IProfileExpression ProfileExpression { private get; set; }

            void IProfileExpression.DisableConstructorMapping() => this.ProfileExpression.DisableConstructorMapping();

            IMappingExpression<TSource, TDestination> IProfileExpression.CreateMap<TSource, TDestination>()
                => this.ProfileExpression.CreateMap<TSource, TDestination>();

            IMappingExpression<TSource, TDestination> IProfileExpression.CreateMap<TSource, TDestination>(MemberList memberList)
                => this.ProfileExpression.CreateMap<TSource, TDestination>(memberList);

            void IProfileExpression.ClearPrefixes() => this.ProfileExpression.ClearPrefixes();

            void IProfileExpression.RecognizePrefixes(params string[] prefixes) => this.ProfileExpression.RecognizePrefixes(prefixes);

            void IProfileExpression.RecognizePostfixes(params string[] postfixes) => this.ProfileExpression.RecognizePostfixes(postfixes);

            void IProfileExpression.RecognizeAlias(string original, string alias) => this.ProfileExpression.RecognizeAlias(original, alias);

            void IProfileExpression.ReplaceMemberName(string original, string newValue) => this.ProfileExpression.ReplaceMemberName(original, newValue);

            void IProfileExpression.RecognizeDestinationPrefixes(params string[] prefixes) => this.ProfileExpression.RecognizeDestinationPrefixes(prefixes);

            void IProfileExpression.RecognizeDestinationPostfixes(params string[] postfixes) => this.ProfileExpression.RecognizeDestinationPostfixes(postfixes);

            void IProfileExpression.AddGlobalIgnore(string propertyNameStartingWith) => this.ProfileExpression.AddGlobalIgnore(propertyNameStartingWith);

            IMemberConfiguration IProfileExpression.AddMemberConfiguration() => this.ProfileExpression.AddMemberConfiguration();

            IConditionalObjectMapper IProfileExpression.AddConditionalObjectMapper() => this.ProfileExpression.AddConditionalObjectMapper();

            bool IProfileExpression.AllowNullDestinationValues
            {
                get { return this.ProfileExpression.AllowNullDestinationValues; }
                set { this.ProfileExpression.AllowNullDestinationValues = value; }
            }

            bool IProfileExpression.AllowNullCollections
            {
                get { return this.ProfileExpression.AllowNullCollections; }
                set { this.ProfileExpression.AllowNullCollections = value; }
            }

            INamingConvention IProfileExpression.SourceMemberNamingConvention
            {
                get { return this.ProfileExpression.SourceMemberNamingConvention; }
                set { this.ProfileExpression.SourceMemberNamingConvention = value; }
            }

            INamingConvention IProfileExpression.DestinationMemberNamingConvention
            {
                get { return this.ProfileExpression.DestinationMemberNamingConvention; }
                set { this.ProfileExpression.DestinationMemberNamingConvention = value; }
            }

            bool IProfileExpression.CreateMissingTypeMaps
            {
                get { return this.ProfileExpression.CreateMissingTypeMaps; }
                set { this.ProfileExpression.CreateMissingTypeMaps = value; }
            }

            string IProfileExpression.ProfileName => this.ProfileExpression.ProfileName;

            public Func<PropertyInfo, bool> ShouldMapProperty
            {
                get { return this.ProfileExpression.ShouldMapProperty; }
                set { this.ProfileExpression.ShouldMapProperty = value; }
            }

            public Func<FieldInfo, bool> ShouldMapField
            {
                get { return this.ProfileExpression.ShouldMapField; }
                set { this.ProfileExpression.ShouldMapField = value; }
            }

            void IProfileExpression.IncludeSourceExtensionMethods(Assembly assembly) => this.ProfileExpression.IncludeSourceExtensionMethods(assembly);

            void IProfileExpression.ForAllMaps(Action<TypeMap, IMappingExpression> configuration) => this.ProfileExpression.ForAllMaps(configuration);

            IMappingExpression IProfileExpression.CreateMap(Type sourceType, Type destinationType, MemberList memberList) => this.ProfileExpression.CreateMap(sourceType, destinationType, memberList);

            IMappingExpression IProfileExpression.CreateMap(Type sourceType, Type destinationType) => this.ProfileExpression.CreateMap(sourceType, destinationType);

            public IMappingExpression CreateMap(Type sourceType, Type destinationType) => this.ProfileExpression.CreateMap(sourceType, destinationType);

            public IMappingExpression CreateMap(Type sourceType, Type destinationType, MemberList memberList) => this.ProfileExpression.CreateMap(sourceType, destinationType, memberList);

            public void ForAllMaps(Action<TypeMap, IMappingExpression> configuration) => this.ProfileExpression.ForAllMaps(configuration);

            public void IncludeSourceExtensionMethods(Assembly assembly) => this.ProfileExpression.IncludeSourceExtensionMethods(assembly);
        }

        private class MapperWrapper : IMapper
        {
            internal IMapper Mapper { private get; set; }

            TDestination IMapper.Map<TDestination>(object source) => this.Mapper.Map<TDestination>(source);

            TDestination IMapper.Map<TDestination>(object source, Action<IMappingOperationOptions> opts) => this.Mapper.Map<TDestination>(source, opts);

            TDestination IMapper.Map<TSource, TDestination>(TSource source) => this.Mapper.Map<TSource, TDestination>(source);

            TDestination IMapper.Map<TSource, TDestination>(TSource source, Action<IMappingOperationOptions<TSource, TDestination>> opts) => this.Mapper.Map(source, opts);

            TDestination IMapper.Map<TSource, TDestination>(TSource source, TDestination destination) => this.Mapper.Map(source, destination);

            TDestination IMapper.Map<TSource, TDestination>(TSource source, TDestination destination, Action<IMappingOperationOptions<TSource, TDestination>> opts) => this.Mapper.Map(source, destination, opts);

            object IMapper.Map(object source, Type sourceType, Type destinationType) => this.Mapper.Map(source, sourceType, destinationType);

            object IMapper.Map(object source, Type sourceType, Type destinationType, Action<IMappingOperationOptions> opts) => this.Mapper.Map(source, sourceType, destinationType, opts);

            object IMapper.Map(object source, object destination, Type sourceType, Type destinationType) => this.Mapper.Map(source, destination, sourceType, destinationType);

            object IMapper.Map(object source, object destination, Type sourceType, Type destinationType, Action<IMappingOperationOptions> opts) => this.Mapper.Map(source, destination, sourceType, destinationType, opts);

            IConfigurationProvider IMapper.ConfigurationProvider => this.Mapper.ConfigurationProvider;
        }

        private class MappingEngineWrapper : IMappingEngine
        {
            internal IMappingEngine MappingEngine { private get; set; }

            bool IMappingEngine.ShouldMapSourceValueAsNull(ResolutionContext context) => this.MappingEngine.ShouldMapSourceValueAsNull(context);

            bool IMappingEngine.ShouldMapSourceCollectionAsNull(ResolutionContext context) => this.MappingEngine.ShouldMapSourceCollectionAsNull(context);

            object IMappingEngine.CreateObject(ResolutionContext context) => this.MappingEngine.CreateObject(context);

            object IMappingEngine.Map(ResolutionContext context) => this.MappingEngine.Map(context);

            IConfigurationProvider IMappingEngine.ConfigurationProvider => this.MappingEngine.ConfigurationProvider;

            IMapper IMappingEngine.Mapper => this.MappingEngine.Mapper;
        }
    }
}
