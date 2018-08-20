import React from 'react'
import { connect } from 'react-redux'
import { Row, Col, Button } from 'react-bootstrap'
import NodeRoles from './nodes/NodeRoles'
import { actionCreators as signalr } from '../SignalRMiddleware'

const Nodes = props => {
    let emptyGuid = '00000000-0000-0000-0000-000000000000'
    let noBout = props.nodes.list.filter((e, i) => { return e.boutId === emptyGuid })
    if (noBout.length > 0) console.log(noBout)

    return (
        <div>
            {noBout.length > 0 ? <div>
                <h2>Waiting For Bout Assignment</h2>
                {noBout.map((e, i) => <Row key={i}>
                    <Col sm={1}>Node:{e.connectionNumber}</Col>
                    {
                        props.bout.running.map((bout, boutidx) => <Col key={boutidx} sm={1}>
                            <Button onClick={() => props.addToBout(e.nodeId, bout.boutId)}>{bout.name}</Button>
                        </Col>)
                    }
                </Row>)}
            </div> : null}
            {
                props.bout.running.length === 0
                    ? <h1>No Bouts Running</h1>
                    : props.bout.running.map((bout, boutidx) => <div key={boutidx}>
                        <h2>Bout: {bout.name}</h2>
                        <div>
                            {
                                props.nodes.list.filter((e, i) => { return e.boutId === bout.boutId })
                                    .map((node, nodeidx) => <NodeRoles node={node} key={nodeidx} />)
                            }
                        </div>
                    </div>
                    )
            }
            {
                props.nodes.list.length === 0
                    ? <h1>No Nodes Connected</h1>
                    : null
            }
        </div>
    )
};

const mapStateToProps = state => {
    return {
        bout: state.bout,
        nodes: state.nodes
    }
}

const mapDispatchToProps = dispatch => {
    return {
        addToBout: (nodeId, boutId) => dispatch(signalr.addToBout(nodeId, boutId))
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(Nodes);
