namespace FastSlnPresentation.Server.Extensions
{
    public static class RedirectExtensions
    {
        public static IApplicationBuilder UseRootRedirect(
            this IApplicationBuilder app,
            string redirectPath
        )
        {
            app.Use(
                async (context, next) =>
                {
                    if (context.Request.Path == "/")
                    {
                        context.Response.Redirect(redirectPath);
                        return;
                    }

                    await next();
                }
            );

            return app;
        }
    }
}
