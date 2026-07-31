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

        Both corpora group chapters under stories (list_stories / get_stories). A chapter's
        StoryId 0 is "(Unassigned)" — a legal, permanent state, not an error. Chapter labels are
        story-qualified ("TLTT CH#12"). The two corpora's Stories are never joined or
        cross-referenced: a story of the same name in the working plan and the archive are
        unrelated rows with no shared id, exactly like subjects and notes.

        Subjects and plot points carry a THEATER (list_theaters) — the master timeline's x-axis,
        a display coordinate ordered by narrative density, not a taxonomy. TheaterId 0 is
        "(Unplaced)": the author has not placed it, often because placement is genuinely
        undecided. Placement is authorial — never infer or propose it from a subject's name.
        Pivots are authored years where the world's causal regime changed; eras are DERIVED as
        the gaps between consecutive pivots and are never stored or named.

        Flagged notes are open questions, not settled lore: ordinary tools never return their
        content or flag reasons, and instead disclose per-track flagged tallies. Retrieve them
        deliberately via list_open_questions / get_open_questions (flag reasons often contain
        substantial draft thinking and are regex-searchable there).

        Typical flow: search_* (full .NET regex; alternation like "Coltbert|crossbow" works) ->
        fetch by ids (get_notes_*/get_subjects_*/...) -> get_track_definitions for the tracks
        present -> follow edge ids embedded in fetch results (subject fetches list scene links;
        plot point fetches list linked subjects and chapter). count_notes_* groups counts across
        dimensions for shape/absence questions.

        World dates are structured (start year/month/day + optional end), rendered in a
        notation: "1007" = event, year precision; "1007-03-15" = pinned to a day; "854..914" =
        interval; "1007.." / "..1007" = interval with an endpoint still to be determined.
        Negative years are BLB (before Luna's banishment); 0 is the banishment. Whether a note's
        date is an event (when it happened) or a condition (over what period it held) is
        determined by its TRACK (event tracks vs condition tracks — see get_track_definitions'
        "worldDate (event…)" / "worldDate (condition…)" flags), never by the value alone.
        Files not yet converted may still carry legacy free-text dates ("993", "870-928"); tools
        convert those mechanically on read and label unconvertible values "(unparsed)" — never
        guessed. Undated is a valid, long-lived authorial state, not an error.
        """;
}
