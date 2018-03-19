import React from 'react'

class App extends React.Component{
    render(){
        return  (
            <div>
                <h4>{this.props.headerProp}</h4>
                <p>{this.props.contentProp}</p>
            </div>
        );
    }
}

App.defaultProps={
headerProp:"This is the header.",
contentProp:"Standing in the hall of fame ..and the world is gonna know your name."
}

export default App;