namespace StoryPlanner.Mcp;

/// <summary>
/// Server instructions sent to clients at initialization. Data-descriptive only:
/// what the corpora are and how the files' semantics differ. No role or workflow
/// instructions — intelligence lives in the client, not in this server.
/// </summary>
public static class ServerInfo
{
    public const string Instructions =
        """
        Read-only retrieval over "The Lioness of Tall Tale" (TLTT) story-planning data — an
        MLP:FiM x Equestria at War (HoI4 mod) fanfiction plan. Three independent corpora, never
        joined by the tools:

        - WORKING plan (v2, *_plan tools): the current plan. Subjects are typed (Character, Bond,
          Organization, Civilizational System, Technology, World Law). Notes belong to a typed
          track that answers exactly one question (get_track_definitions). Note states: unset
          (captured, not reviewed), confirmed (stable), flagged (open question — walled off).
        - ARCHIVE (v1, *_archive tools): the older capture-era dataset. Deliberately different
          organization: no tracks, and a much richer scene graph (plot-point x subject links).
          States: open / flagged / closed. "Closed" means reviewed and no longer needing
          attention — whether the content was migrated to v2 or deliberately superseded was NOT
          recorded, so never treat closed archive notes as either current or migrated.
        - CONVERSATIONS: imported AI chat transcripts (search_conversations / get_blocks), with
          the author's per-block read states (unread / skipped / flagged / done).

        Flagged notes are open questions, not settled lore: ordinary tools never return their
        content or flag reasons, and instead disclose per-track flagged tallies. Retrieve them
        deliberately via list_open_questions / get_open_questions (flag reasons often contain
        substantial draft thinking and are regex-searchable there).

        Typical flow: search_* (full .NET regex; alternation like "Coltbert|crossbow" works) ->
        fetch by ids (get_notes_*/get_subjects_*/...) -> get_track_definitions for the tracks
        present -> follow edge ids embedded in fetch results (subject fetches list scene links;
        plot point fetches list linked subjects and chapter). count_notes_* groups counts across
        dimensions for shape/absence questions. WorldDate is free text (years or ranges, e.g.
        "993", "-100-0") returned raw plus a mechanical start/end parse.
        """;
}
