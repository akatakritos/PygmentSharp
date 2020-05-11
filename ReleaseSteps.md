## Release Steps

### Things to check before releasing
 - Tests pass?
 - Public API documented?
 - Roadmap up to date?
 - Does it build and pass tests in Release mode?


### Steps

1. Cut release branch from develop
2. Bump versions in `PygmentSharp.Core.csproj`
3. Update changelog
4. Update nuget release notes in project properties, Package section
5. PR release branch to master
6. If CI passes, merge PR to master
7. Tag master with new version
8. Build nuget: `nuget pack -c Release`
9. Upload new package to nuget website
10. Merge release branch back into develop
11. Delete release branch