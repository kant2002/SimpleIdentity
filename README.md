Simple Identity
===============

This application shows one scenario which not very well covered by ASP.NET Identity Core in my opinion.

# Assumptions
- There existing database for storing login information.
- There lowered security requirements from the client, and it not see all modern security as very important
- Application is limited to small number (<10) users within same small org.
- All user validation logic happens on the DB

# Goals

- Do not try to change mind of other people who have some architectural predisposition
- Provide as much security as possible without rolling your own custom solution
- Do not mess with complexity of store interfaces.

# Solution

Instead of writing custom persistence stores which seems to be require you to implement all of them, 
I propose much simpler variant where you override `UserManager<T>`.
Because developer write code using that class, they are much more familiar with these API, it's immidiately
clear what method you should override, and all these customizations contained to single class.

Cookies and all security tokens still created by ASP.NET Identity and all check still performed by ASP.NET AuthZ/AuthN pipeline.

# Example

= Make sure that SSDT installed in Visual Studio, otherwise you have to manually create DB and publish scripts.
- Open solution in Visual Studio
- Expand SimpleIdentity.Db project and double-click on Debug.publish.xml
- Set SimpleIdentity as startup project and run.

Minimal work to supprort this scenario is to have following classes.

- `UserManager`
- `SimpleSignInManager`
- `IdentityContext` just to trick Identity.

Other notes
- You do not even need custom `ApplicationUser` if fields from `IdentityUser` is enough for you. 
- You do not derive your `DbContext` from IdentityDbContext since it would not be used.
- You probably would not need `IdentityContext` without `ApplicationUser` and you can use `IdentityDbContext<IdentityUser>` directly in the Program.cs
