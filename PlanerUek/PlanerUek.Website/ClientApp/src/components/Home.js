import React, { Component } from 'react';
import Api from "../utils/api";

export class Home extends Component {
    state = {
        groupName: '',
        userId: ''
    };
    
    componentDidMount() {
        if (this.props.location.search.includes("refresh=true")){
            Api.post("/api/StudentGroups/HandleTimetableForClient", {}).then(result => console.log(result));
        }
    }

    onTextInput = event => {
        const value = event.target.value;
        this.setState({[event.target.name]: value});
    };
    
    addScheduleForGroup = () => {
        if (this.state.groupName.length === 0 || this.state.userId.length === 0){
            return;
        }
        Api.post("/api/StudentGroups/HandleTimetableForClient", {...this.state})
            .then(result => {
                console.log(result);
                window.location = result.authorizationEndpoint;
            })
            .catch(reason => console.log(reason.message));
    };
    
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
          <div className="input-group">
              <div className="input-group-prepend">
                  <span className="input-group-text" id="">Your Google account email</span>
              </div>
              <input onChange={this.onTextInput} className="form-control" name="userId" type="text"/>
              <div className="input-group-prepend">
                  <span className="input-group-text" id="">Your student group name</span>
              </div>
              <input onChange={this.onTextInput} className="form-control" name="groupName" type="text"/>
              <div className="input-group-append">
                  <button className="btn btn-primary" type="button" onClick={this.addScheduleForGroup}>Go!</button>
              </div>
          </div>
      </div>
    );
  }
}
