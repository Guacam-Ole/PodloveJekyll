﻿---
episode template

generator: "Feed2Md"
created: "[Global.Created]"
podcast: "[Podcast.Title]"
title: "[Episode.Title]"
link: "[Episode.Link]"
published: "[Episode.Published]"
episode: "[Episode.Itunes.Episode]"
Season: "[Episode.Itunes.Episode]"
---

# [Episode.Title]
_generated by feed2md (test) on [Global.Created]_

[Episode.Description]

\[Play Episode\]([Episode.AudioUrl])

Brought to you by: [Podcast.Itunes.Author]

This Episode contains [Episode.Chapters.Count] Chapters:

[LOOP.Episode.Chapters]
[Episode.Chapters.Chapter.Duration] \[[Episode.Chapters.Chapter.Title]\](Episode.Chapters.Chapter.Url)
[POOL]
