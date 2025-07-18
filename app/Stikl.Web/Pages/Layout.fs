module Stikl.Web.Pages.Layout


open System
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.DependencyInjection
open Stikl.Web
open Stikl.Web.Components
open domain
open Stikl.Web.services.User

let header (user: User Option) (locale: Localization) =
    let profileButton =
        // TODO: check expired (possibly in the principal level)
        match user with
        | Some user ->
            // language=HTML
            $"""
            <a
                class="transform px-3 py-1 font-sans text-sm font-bold {Theme.textBrandColor} underline transition"
                href="/auth/profile"
            >
                {user.fullName |> locale.hi}	
            </a>
            """
        | None ->
            // language=HTML
            $"""
            <a
                class="{Theme.smButton}"
                hx-boost="false"
                href="/auth/login"
            >
               {locale.logIn} 
            </a>
            """

    // language=HTML
    $"""
    <header class="flex justify-between p-2">
        <div class="flex gap-4">
            <a
                class="rounded-lg {Theme.themeBgGradient} px-3 py-1 text-left font-sans text-xl font-semibold text-white hover:underline"
                href="/"
            >
                Stikl.dk
            </a>
            <div id="spinner" role="status" class="htmx-indicator">
                {Spinner.render}
            </div>
        </div>
        <div class="flex justify-between gap-5">
            {profileButton}
        </div>
    </header>
    """

let modalId = "modals-here"

let render content (user: User Option) (locale: Localization) =
    // language=html
    $"""
	<!doctype html>
    <html lang="da" class="bg-[url(/img/leaf.svg)] overflow-none" style="background-size: 100px">
      <head>
        <meta charset="utf-8" />
        <meta name="viewport" content="width=device-width, initial-scale=1" />
        <script src="https://cdn.jsdelivr.net/npm/htmx.org@2.0.6/dist/htmx.min.js" crossorigin="anonymous"></script>
        <script src="https://cdn.jsdelivr.net/npm/htmx-ext-sse@2.2.2"  crossorigin="anonymous"></script>
        <script src="https://unpkg.com/hyperscript.org@0.9.13"></script>
        <script src="https://kit.fontawesome.com/ab39de689b.js" crossorigin="anonymous"></script>
        <title>Stikl.dk</title>
        <script src="https://cdn.tailwindcss.com"></script>
      </head>
      <body hx-ext="sse" class="flex flex-col justify-between h-screen" hx-boost="true" hx-indicator="#spinner">
        <div id="{modalId}"></div>
		{header user locale}
        <main class="container relative mx-auto mt-10 flex flex-grow flex-col items-center mb-auto space-y-8 p-2">
          <div id="messages" class="absolute top-0 right-0 md:right-10 grid gap-4 w-32 lg:w-64"></div>
          {content}
        </main>
        <footer class="flex w-full items-center justify-between p-4 {Theme.textMutedColor}">
          <p class="text-sm">© {DateTimeOffset.UtcNow.Year} stikl.dk. All rights reserved.</p>
        </footer>
      </body>
    </html>
"""
    |> Result.Html.Ok

type Builder = { render: string -> IResult }

let register (s: IServiceCollection) =
    s.AddScoped<Builder>(fun s ->
        let currentUser = s.GetRequiredService<CurrentUser>()
        let locale = Localization.``default``

        { render = fun content -> render content currentUser.get locale })
