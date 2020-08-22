import React from 'react'
import { Redirect, useParams } from 'react-router-dom'
import { connect } from 'react-redux';


const ProtectedRoute = (props) => {
    const Component = props.component;
    const isAuthenticated = props.user.token !== "";

    const { params } = props.computedMatch;
    const { location } = props.location;

    return isAuthenticated ? (
        <Component params={params} location={location}/>
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