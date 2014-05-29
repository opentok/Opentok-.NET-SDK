﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using OpenTokSDK.Util;

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
         * The archive recording has started and is in progress.
         */
        STARTED,
        /**
         * The archive recording has stopped, but the file is not available.
         */
        STOPPED,
        /**
         * The archive file is available at the target S3 bucket you set at the
         * <a href="https://dashboard.tokbox.com">OpenTok dashboard</a>.
         */
        UPLOADED,
        /**
         * The status of the archive is unknown.
         */
        UNKOWN
    }

    /**
    * Represents an archive of an OpenTok session.
    */
    public class Archive
    {               

        private OpenTok opentok;

        protected Archive ()
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
            this.Id = archive.Id;
            this.Name = archive.Name;
            this.PartnerId = archive.PartnerId;
            this.SessionId = archive.SessionId;
            this.Size = archive.Size;
            this.Status = archive.Status;
            this.Url = archive.Url;
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


        public String Reason { get; set; }  
        
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
         * <p>
         * Archives automatically stop recording after 90 minutes or when all clients have disconnected from the
         * session being archived.
         */
        public void Stop()
        {
            if(opentok != null)
            {
                Archive archive = opentok.StopArchive(Id.ToString());
                Status = archive.Status;
            }
        }

        /**
         * Deletes the OpenTok archive.
         * <p>
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