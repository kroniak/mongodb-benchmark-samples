# Mongodb transaction vs non-transaction operation benchmarks

## Install

Use scripts in folder ./runner for docker run
run.mongodb.sh
run.postgres.sh

Use script ./runner/test.sh for run test

## Results

[Testing result for MultiDocs](BenchmarkDotNet.Artifacts/results/MongodbTransactions.TestCases.MultiDocs-report-github.md)

[Testing result for MultiCollection](BenchmarkDotNet.Artifacts/results/MongodbTransactions.TestCases.MultiCollections-report-github.md)

[Testing result for MultiCollectionUpdate](BenchmarkDotNet.Artifacts/results/MongodbTransactions.TestCases.MultiCollectionsUpdate-report-github.md)

[Testing result for compare Postgres and Mongo inserting data](BenchmarkDotNet.Artifacts/results/MongodbTransactions.CompareTestCases.CompareMultiDocs-report-github.md)

[Testing result for concurrency Mongo update with transactions](BenchmarkDotNet.Artifacts/results/MongodbTransactions.ConcurrentTestCases.MultiCollectionsConcurrentUpdate-report-github.md)

[Testing result for validation Mongo insert](BenchmarkDotNet.Artifacts/results/MongodbTransactions.TestCases.SchemaValidator-report-github.md)

[Blog Scenario 1 result](BenchmarkDotNet.Artifacts/results/MongodbTransactions.Blog.BlogNestedDocuments-report-github.md)

[Blog Scenario 2 result](BenchmarkDotNet.Artifacts/results/MongodbTransactions.Blog.BlogSeparateDocuments-report-github.md)

[NoSQl Scenario result](BenchmarkDotNet.Artifacts/results/MongodbTransactions.NoSql.NoSqlTest-report-github.md)
