﻿module domain

open System

type PlantId = Guid

module PlantId =
    let create () = Guid.NewGuid()

// We need to figure out what type of user id - dotnet-jwt gives a string of the username. auth0 probably same
type UserId =
    | UserId of string

    member this.value =
        match this with
        | UserId value -> value


module UserId =
    // These
    let random = Guid.NewGuid().ToString() |> UserId

type UserEvent =
    | AddedWant of PlantId
    | AddedSeeds of PlantId
    | RemovedWant of PlantId
    | RemovedSeeds of PlantId

type User =
    { id: UserId
      wants: PlantId Set
      seeds: PlantId Set
      history: UserEvent List }


module User =
    let Wants plantId user = Set.contains plantId user.wants

    let Has plantId user = Set.contains plantId user.seeds

    let GetWants user = user.wants

    let GetSeeds user = user.seeds

    let create id =
        { id = id
          wants = Set.empty
          seeds = Set.empty
          history = List.empty }


    let createRandom () = create UserId.random

let apply (event: UserEvent) (user: User) =
    let user =
        (match event with
         | AddedWant plantId ->
             { user with
                 wants = Set.add plantId user.wants }
         | AddedSeeds plantId ->
             { user with
                 seeds = Set.add plantId user.seeds }
         | RemovedWant plantId ->
             { user with
                 wants = Set.remove plantId user.wants }
         | RemovedSeeds plantId ->
             { user with
                 seeds = Set.remove plantId user.seeds })

    { user with
        history = event :: user.history }
