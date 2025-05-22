using System.ComponentModel;

namespace TechStax.Models
{
    public enum Technology
    {
        [Description("C#")]
        CSharp,
        [Description(".NET")]
        DotNet,
        [Description("ASP.NET Core")]
        AspNetCore,

        Java,
        [Description("Spring")]
        Spring,
        [Description("Spring Boot")]
        SpringBoot,

        Python,
        [Description("Django")]
        Django,
        [Description("Flask")]
        Flask,

        JavaScript,
        [Description("TypeScript")]
        TypeScript,
        [Description("Node.js")]
        NodeJS,
        [Description("React")]
        React,
        [Description("Angular")]
        Angular,
        [Description("Vue.js")]
        VueJS,
        [Description("Svelte")]
        Svelte,
        [Description("Ember.js")]
        EmberJs,
        [Description("Backbone.js")]
        BackboneJs,
        [Description("Next.js")]
        NextJs,
        [Description("Nuxt.js")]
        NuxtJs,
        [Description("Gatsby")]
        Gatsby,
        [Description("Electron")]
        Electron,
        [Description("React Native")]
        ReactNative,

        [Description("GraphQL")]
        GraphQL,
        [Description("REST")]
        Rest,
        [Description("gRPC")]
        Grpc,
        [Description("Express.js")]
        ExpressJs,
        [Description("NestJS")]
        NestJs,

        [Description("GoLang")]
        GoLang,
        [Description("Rust")]
        Rust,
        [Description("Scala")]
        Scala,
        [Description("Kotlin")]
        Kotlin,
        [Description("Swift")]
        Swift,
        [Description("SwiftUI")]
        SwiftUI,
        [Description("Objective-C")]
        ObjectiveC,
        [Description("Elixir")]
        Elixir,
        [Description("Erlang")]
        Erlang,
        [Description("Haskell")]
        Haskell,
        [Description("F#")]
        FSharp,

        [Description("C++")]
        Cpp,

        Ruby,
        [Description("Ruby on Rails")]
        RubyOnRails,
        [Description("PHP")]
        Php,
        [Description("Laravel")]
        Laravel,
        [Description("Symfony")]
        Symfony,

        SQL,
        [Description("PostgreSQL")]
        PostgreSQL,
        [Description("MySQL")]
        MySql,
        [Description("SQLite")]
        SQLite,
        [Description("MariaDB")]
        MariaDb,
        [Description("Oracle")]
        Oracle,
        [Description("SQL Server")]
        SqlServer,
        [Description("MongoDB")]
        MongoDb,
        Redis,
        CouchDb,
        Cassandra,
        [Description("Neo4j")]
        Neo4j,
        [Description("DynamoDB")]
        DynamoDb,
        Firebase,
        [Description("InfluxDB")]
        InfluxDb,
        [Description("Couchbase")]
        Couchbase,

        AWS,
        [Description("AWS S3")]
        AwsS3,
        [Description("AWS EC2")]
        AwsEc2,
        [Description("AWS Lambda")]
        AwsLambda,
        [Description("AWS ECS")]
        AwsEcs,
        [Description("AWS EKS")]
        AwsEks,
        Azure,
        [Description("Azure Functions")]
        AzureFunctions,
        [Description("Azure DevOps")]
        AzureDevOps,
        GoogleCloud,
        [Description("Google Cloud Functions")]
        GoogleCloudFunctions,

        Docker,
        [Description("Docker Compose")]
        DockerCompose,
        Podman,
        Kubernetes,
        Helm,
        Istio,
        Nomad,
        Mesos,

        Terraform,
        Ansible,
        Chef,
        Puppet,
        [Description("Packer")]
        Packer,
        [Description("Vagrant")]
        Vagrant,

        Kafka,
        RabbitMq,
        Prometheus,
        Grafana,
        [Description("Elasticsearch")]
        Elasticsearch,
        [Description("Logstash")]
        Logstash,
        [Description("Fluentd")]
        Fluentd,
        [Description("Graylog")]
        Graylog,

        Hadoop,
        Spark,

        Nginx,
        Apache,
        IIS,

        Git,
        [Description("GitHub Actions")]
        GitHubActions,
        [Description("GitLab CI")]
        GitLabCi,
        [Description("Travis CI")]
        TravisCi,
        [Description("CircleCI")]
        CircleCi,
        Jenkins,
        [Description("TeamCity")]
        TeamCity,
        [Description("Bamboo")]
        Bamboo,
        [Description("Concourse CI")]
        ConcourseCi,
        [Description("Argo CD")]
        ArgoCd,

        [Description("Jest")]
        Jest,
        [Description("Mocha")]
        Mocha,
        [Description("Jasmine")]
        Jasmine,
        [Description("Cypress")]
        Cypress,
        [Description("Selenium")]
        Selenium,
        [Description("JUnit")]
        JUnit,
        [Description("NUnit")]
        NUnit,
        [Description("xUnit")]
        XUnit,
        [Description("pytest")]
        Pytest,
        [Description("RSpec")]
        RSpec,

        [Description("npm")]
        Npm,
        [Description("Yarn")]
        Yarn,
        [Description("pip")]
        Pip,
        [Description("Maven")]
        Maven,
        [Description("Gradle")]
        Gradle,
        [Description("NuGet")]
        NuGet,
        [Description("Composer")]
        Composer,
        [Description("Bundler")]
        Bundler,

        DataDog,
        [Description("New Relic")]
        NewRelic,
        Splunk,
        Zabbix,
        Nagios
    }
}
