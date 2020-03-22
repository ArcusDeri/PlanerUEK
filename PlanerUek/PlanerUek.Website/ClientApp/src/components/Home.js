import React, { Component } from 'react';

export class Home extends Component {
  static displayName = Home.name;

  render () {
    return (
      <div>
        <h1>Hello, world!</h1>
            <form method="POST" action="/api/StudentGroups/HandleTimetableForGroup">
                <label htmlFor="groupName">Student group name</label>
                <input type="text" name="groupName"/>
                <input type="submit"/>
            </form>
      </div>
    );
  }
}
