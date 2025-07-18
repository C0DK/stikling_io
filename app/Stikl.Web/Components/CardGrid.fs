module Stikl.Web.Components.CardGrid

let render (cards: string seq) =
    //language=HTML
    $"""
     <div class="grid md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-4 mb-4 justify-center">
        {cards |> String.concat "\n"}
     </div>
     """
