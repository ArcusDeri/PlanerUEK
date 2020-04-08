import React, { Component } from 'react';

export class Home extends Component {
  static displayName = Home.name;

  render () {
    return (
      <div>
        <p>
            This application integrates your student group schedule with your Google Calendar. Type in your student group name
            (the same you can find at <a href="http://planzajec.uek.krakow.pl/">planzajec.uek.krakow.pl</a>). The application
            will map events in timetable to Google Calendar events and save them in your calendar.
            
            You will be prompted to sign into your Google account along the way. This application doesn't store any details about
            your Google account.
        </p>
          <strong>Try submitting this group: KrDzIs3011Io</strong>
        <form method="POST" action="/api/StudentGroups/HandleTimetableForGroup">
            <label htmlFor="groupName">Student group name</label>
            <input type="text" name="groupName"/>
            <input type="submit"/>
        </form>
      </div>
    );
  }
}
