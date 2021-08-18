# Process of work

[Back](../Readme.md)

In order to facilitate parallel work each bug fix or feature will be its own branch


### Example Issues

```
Issue #1 Add Google OAuth
        Tagged as FEATURE
        Allow users to authenticate using their google accounts

Issue #2 Add Github OAuth
        Tagged as FEATURE
        Allow users to authenticate using their github accounts
        
Issue #3 Fix LinkedIn Claims for users
        Tagged as BUG
        Certain info such as linkedin username/profile link are not carried over
```

## Branches
Once you identify an issue you want to work on - be sure to pull the latest version of master

The following naming conventions shall be followed:

**Feature Issues** 
```
Working on issue #1 (feature = f)

F1_GoogleOAuth
```

**Bug Issues** - BF for bug fix
```
Working on issue #3 (bug fix = bf)
BF3_LinkedInClaims
```

Feel free to commit your work as much as you want. When you're at a comfortable state
you can create a **pull request** (PR) targeting master.

-----

# Pull Requests
Please be sure to assign yourself as the assignee

In general you must follow the criteria below

### Tests
Every feature, unless the general consensus is it's not required, requires unit testing.
This ensures the feature is working as expected and that future work doesn't accidently break things.

For every bug fix -- you must ensure a unit test is either added or corrected to ensure the bug doesn't reappear in production.

### Documentation
Your code should be well documented. No - you don't need every line commented. Any developer who follows behind you should be able
to easily determine what's going on without asking a million questions.

### Pipeline Checks
Your branch **MUST** pass the automated pipeline. If a unit test is faulty - you must bring it up to the community. Otherwise,
directly modifying a test to simply pass the pipeline will be denied. Those tests are there for a reason.

### Code Reviews
Everyone is expected to behave in a professional manner. No one is perfect. A code review is common practice in the software development
industry. Perhaps an industry standard wasn't followed (naming conventions for instance), or 
a more optimal approach exists, or a particular use case wasn't thought of.  

These reviews are meant to act not only as a learning experience to some but to ensure the code going into
production is the best possible version we can deploy.

All peer comments must be resolved.

### Release State
If all the above criteria are met and passing - if the issue you worked on is slated for the current release
it will get merged.

If your work is not slated for a future release it will be labeled as "Ready for Next Release". Once the current
release is cut - your PR will get merged.

[Back](../Readme.md)