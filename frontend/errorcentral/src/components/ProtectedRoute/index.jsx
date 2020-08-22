import React from 'react'
import { Redirect } from 'react-router-dom'
import { connect } from 'react-redux';


const ProtectedRoute = (props) => {
    const Component = props.component;
    const isAuthenticated = props.user.token !== "";

    return isAuthenticated ? (
        <Component />
    ) : (
        <Redirect to={{ pathname: '/login' }} />
    );
}


const mapStateToProps = (state) => {
    return {
        user: state.user
    }
}

export default connect(mapStateToProps, null)(ProtectedRoute);;