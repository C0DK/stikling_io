module webapp.Components

open domain

let search =
    """
<span class="htmx-indicator">
    Searching...
</span>
<input class="form-control" type="search"
   name="query" placeholder="Begin Typing To Search Users..."
   hx-get="/search"
   hx-trigger="input changed delay:500ms, keyup[key=='Enter']"
   hx-target="#search-results"
   hx-indicator=".htmx-indicator">

<div id="search-results" class="grid grid-cols-3 gap-4">
</div>
"""

let plantCard (plant: Plant) =
    $"""
<div
	class="h-80 w-64 max-w-sm rounded-lg border border-gray-200 bg-white shadow"
>
	<img
		alt="Image of {plant.name}"
		class="h-3/4 w-64 rounded-t-lg border-b-2 border-gray-800 object-cover"
		src={plant.image_url}
	/>
	<div class="float-right mr-2 mt-2 flex flex-col space-y-4">
		<a
			class="cursor-pointer text-sm text-lime-600 underline hover:text-lime-400"
			href="/plant/{plant.id}">Læs mere</a
		>
	</div>
	<h4 class="text-l mb-2 h-auto p-2 italic text-gray-600">
		{plant.name}
	</h4>
</div>
"""

let grid (innerHtml: string) =
    $"""
<div class="grid grid-cols-3 gap-4">
    {innerHtml}
</div>
"""

let themeGradiantSpan innerHtml =
    Htmx.block
        { tag = "span"
          innerHtml = innerHtml
          class_ =
            "inline-block rounded-lg bg-gradient-to-r from-lime-600 to-amber-600 bg-clip-text p-1 px-2 font-bold text-transparent hover:animate-pulse-size" }
