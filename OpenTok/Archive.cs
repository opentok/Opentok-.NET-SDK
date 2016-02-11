﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenTokSDK
{
    /**
     * Defines values returned by the Status property of an Archive object. See the ListArchives()
     * method of the OpenTok class.
     */
    public enum ArchiveStatus
    {
        /**
         * The archive file is available for download from the OpenTok cloud. You can get the URL of
         * the download file by getting the Url property of the Archive object.
         */
        AVAILABLE,
        /**
         * The archive file has been deleted.
         */
        DELETED,
        /**
         * The recording of the archive failed.
         */
        FAILED,
        /**
         * The archive is in progress and no clients are publishing streams to the session.
         * When an archive is in progress and any client publishes a stream, the status is STARTED.
         * When an archive is PAUSED, nothing is recorded. When a client starts publishing a stream,
         * the recording starts (or resumes). If all clients disconnect from a session that is being
         * archived, the status changes to PAUSED, and after 60 seconds the archive recording stops
         * (and the status changes to STOPPED).
         */
        PAUSED,
        /**
         * The archive recording has started and is in progress.
         */
        STARTED,
        /**
         * The archive recording has stopped, but the file is not available.
         */
        STOPPED,
        /**
         * The archive file is available at the target Amazon S3 bucket or Windows Azure container
         * you set at the <a href="https://dashboard.tokbox.com">OpenTok dashboard</a>.
         */
        UPLOADED,
        /**
         * The archive file is no longer available for download from the OpenTok cloud.
         */
        EXPIRED,
        /**
         * The status of the archive is unknown.
         */
        UNKOWN
    }

    /**
     * Defines values for the OutputMode property of an Archive object.
     */
    public enum OutputMode
    {
        /**
         * All streams in the archive are recorded to a single (composed) file.
         */
        COMPOSED,
        /**
         * Each stream in the archive is recorded to its own individual file.
         */
        INDIVIDUAL
    }

    /**
    * Represents an archive of an OpenTok session.
    */
    public class Archive
    {

        private OpenTok opentok;

        protected Archive()
        {

        }

        internal Archive(OpenTok opentok)
        {
            this.opentok = opentok;
        }

        internal void CopyArchive(Archive archive)
        {
            this.CreatedAt = archive.CreatedAt;
            this.Duration = archive.Duration;
            this.Sha256sum = archive.Sha256sum;
            this.Password = archive.Password;
            this.Id = archive.Id;
            this.Name = archive.Name;
            this.PartnerId = archive.PartnerId;
            this.SessionId = archive.SessionId;
            this.Size = archive.Size;
            this.Status = archive.Status;
            this.Url = archive.Url;
            this.HasVideo = archive.HasVideo;
            this.HasAudio = archive.HasAudio;
            this.OutputMode = archive.OutputMode;
        }

        /**
         * The time at which the archive was created, in milliseconds since the Unix epoch.
         */
        public long CreatedAt { get; set; }

        /**
         * The duration of the archive, in milliseconds.
         */
        public long Duration { get; set; }

        /**
         * SHA-256 hash of the file.
         */
        public string Sha256sum { get; set; }

        /**
         * File password
         */
        public string Password { get; set; }

        /**
         * The archive ID.
         */
        public Guid Id { get; set; }

        /**
         * The name of the archive.
         */
        public string Name { get; set; }

        /**
         * The OpenTok API key associated with the archive.
         */
        public int PartnerId { get; set; }

        /**
         * The session ID of the OpenTok session associated with this archive.
         */
        public String SessionId { get; set; }

        /**
         * For archives with the status "stopped", this can be set to "90 mins exceeded",
         * "failure", "session ended", or "user initiated". For archives with the status "failed",
         * this can be set to "system failure".
         */
        public String Reason { get; set; }

        /**
         * Whether the archive includes a video track (true) or not (false).
         */
        public bool HasVideo { get; set; }

        /**
         * Whether the archive includes an audio track (true) or not (false).
         */
        public bool HasAudio { get; set; }

        /**
         * Whether all streams in the archive are recorded to a single file
         * (<code>OutputMode.COMPOSED</code>) or to individual files
         * (<code>OutputMode.INDIVIDUAL</code>). To record streams to individual
         * files, pass <code>OutputMode.INDIVIDUAL</code> as the <code>outputMode</code>
         * parameter when calling the <code>OpenTok.StartArchive()</code> method.
         */
        public OutputMode OutputMode { get; set; }

        /** 
         * The size of the MP4 file. For archives that have not been generated, this value is set
         * to 0.
         */
        public int Size { get; set; }

        /**
         * The status of the archive, as defined by the ArchiveStatus enum.
         */
        public ArchiveStatus Status { get; set; }

        /**
         * The download URL of the available MP4 file. This is only set for an archive with the
         * status set to ArchiveStatus.AVAILABLE; for other archives, (including archives with the
         * status of ArchiveStatus.UPLOADED) this method returns null. The download URL is
         * obfuscated, and the file is only available from the URL for 10 minutes. To generate a
         * new URL, call the ListArchives() or GetArchive() method of the OpenTok object.
         */
        public String Url { get; set; }

        /**
         * Stops the OpenTok archive if it is being recorded.
         * Archives automatically stop recording after 90 minutes or when all clients have disconnected from the
         * session being archived.
         */
        public void Stop()
        {
            if (opentok != null)
            {
                Archive archive = opentok.StopArchive(Id.ToString());
                Status = archive.Status;
            }
        }

        /**
         * Deletes the OpenTok archive.
         * You can only delete an archive which has a status of "available" or "uploaded". Deleting
         * an archive removes its record from the list of archives. For an "available" archive, it
         * also removes the archive file, making it unavailable for download.
         */
        public void Delete()
        {
            if (opentok != null)
            {
                opentok.DeleteArchive(Id.ToString());
            }
        }
    }
}
